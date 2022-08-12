/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#define STRICT

#include <windows.h>

#include "ImageStream.h"

ImageStream::ImageStream(
  _In_ LPCVOID pvData,
  _In_ DWORD dwDataLen)
{
	m_ulRefCount = 0;

    m_pvData = pvData;
    m_dwDataLen = dwDataLen;

    m_Position.QuadPart = 0;
}

ImageStream::~ImageStream()
{
}

#pragma region "IUnknown"

ULONG STDMETHODCALLTYPE ImageStream::AddRef()
{
    return ::InterlockedIncrement(&m_ulRefCount);
}

ULONG STDMETHODCALLTYPE ImageStream::Release()
{
    if (::InterlockedDecrement(&m_ulRefCount) == 0)
    {
		delete this;

        return 0;
    }

    return m_ulRefCount;
}

HRESULT STDMETHODCALLTYPE ImageStream::QueryInterface(
  const IID& iid,
  void** ppv)
{
    iid;

    // Microsoft undocumented GUID's found passed in
    //
    // {C3933843-C24B-45A2-8298-B462F59DAAF2}
    // {3A55501A-BDCC-4E63-96BC-4DDB6F44CCDD}

    if (ppv == NULL)
    {
        return E_POINTER;
    }

    return E_NOINTERFACE;
}

#pragma endregion

#pragma region "IStream"

HRESULT STDMETHODCALLTYPE ImageStream::Seek(
  _In_ LARGE_INTEGER nAmountMove,
  _In_ DWORD dwOrigin,
  _Out_opt_ ULARGE_INTEGER* pnNewPosition)
{
    HRESULT Result = E_FAIL;

    switch (dwOrigin)
    {
        case STREAM_SEEK_SET:
            m_Position.QuadPart = nAmountMove.QuadPart;

            if (pnNewPosition)
            {
                pnNewPosition->QuadPart = m_Position.QuadPart;
            }

            Result = S_OK;
            break;
        case STREAM_SEEK_CUR:
            if (nAmountMove.QuadPart >= 0)
            {
                m_Position.QuadPart += nAmountMove.QuadPart;

                if (pnNewPosition)
                {
                    pnNewPosition->QuadPart = m_Position.QuadPart;
                }

                Result = S_OK;
            }
            else
            {
                // Code not safe if amount move is greater than 2 x 63

                if (-nAmountMove.QuadPart <= (LONGLONG)m_Position.QuadPart)
                {
                    m_Position.QuadPart += nAmountMove.QuadPart;
                }
                else
                {
                    m_Position.QuadPart = 0;
                }

                if (pnNewPosition)
                {
                    pnNewPosition->QuadPart = m_Position.QuadPart;
                }

                Result = S_OK;
            }
            break;
        case STREAM_SEEK_END:
            break;
        default:
            Result = STG_E_INVALIDFUNCTION;
            break;
    }

    return Result;
}

HRESULT STDMETHODCALLTYPE ImageStream::SetSize(
  _In_ ULARGE_INTEGER nNewSize)
{
    nNewSize;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::CopyTo(
  _In_ IStream* pStream,
  _In_ ULARGE_INTEGER nAmount,
  _Out_opt_ ULARGE_INTEGER* pcbRead,
  _Out_opt_ ULARGE_INTEGER* pcbWritten)
{
    nAmount;
    pcbRead;
    pcbWritten;

    if (pStream == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::Commit(
  _In_ DWORD dwCommitFlags)
{
    dwCommitFlags;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::Revert(void)
{
    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::LockRegion(
  _In_ ULARGE_INTEGER nOffset,
  _In_ ULARGE_INTEGER nAmount,
  _In_ DWORD dwLockType)
{
    nOffset;
    nAmount;
    dwLockType;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::UnlockRegion(
  _In_ ULARGE_INTEGER nOffset,
  _In_ ULARGE_INTEGER nAmount,
  _In_ DWORD dwLockType)
{
    nOffset;
    nAmount;
    dwLockType;

    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE ImageStream::Stat(
  __RPC__out STATSTG* pStatStorage,
  _In_ DWORD dwStatFlag)
{
    FILETIME FileTime;

    if (pStatStorage == NULL)
    {
        return E_POINTER;
    }

    ::GetSystemTimeAsFileTime(&FileTime);

    ::CopyMemory(&pStatStorage->mtime, &FileTime, sizeof(FileTime));
    ::CopyMemory(&pStatStorage->ctime, &FileTime, sizeof(FileTime));
    ::CopyMemory(&pStatStorage->atime, &FileTime, sizeof(FileTime));

    pStatStorage->type = STGTY_LOCKBYTES;
    pStatStorage->cbSize.QuadPart = m_dwDataLen;
    pStatStorage->grfMode;
    pStatStorage->grfLocksSupported = 0;
    pStatStorage->clsid = CLSID_NULL;
    pStatStorage->grfStateBits;

    switch (dwStatFlag)
    {
        case STATFLAG_DEFAULT:
            // need to set the pwcsName field
            break;
        case STATFLAG_NONAME:
            break;
        case STATFLAG_NOOPEN:
            // according to documentation not implemented
            break;
    }

    return S_OK;
}

HRESULT STDMETHODCALLTYPE ImageStream::Clone(
  __RPC__deref_out_opt IStream** ppStream)
{
    if (ppStream == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

#pragma endregion

#pragma region "ISequentialStream"

HRESULT STDMETHODCALLTYPE ImageStream::Read(
  _Out_writes_bytes_to_(nDataLen, *pcbRead) void* pvData,
  _In_ ULONG nDataLen,
  _Out_opt_ ULONG* pcbRead)
{
    HRESULT Result = E_FAIL;
    ULONG nDataRead;

    if (pvData == NULL)
    {
        return E_POINTER;
    }

    if (m_Position.QuadPart < m_dwDataLen)
    {
        if (m_Position.QuadPart + nDataLen <= m_dwDataLen)
        {
            nDataRead = nDataLen;
        }
        else
        {
            nDataRead = m_dwDataLen - m_Position.LowPart;
        }

        if (pcbRead)
        {
            *pcbRead = nDataRead;
        }

        ::CopyMemory(pvData, ((LPCBYTE)m_pvData) + m_Position.LowPart, nDataRead);

        m_Position.QuadPart += nDataRead;

        Result = S_OK;
    }
    else
    {
    }

    return Result;
}

HRESULT STDMETHODCALLTYPE ImageStream::Write(
  _In_reads_bytes_(nDataLen) const void* pvData,
  _In_ ULONG nDataLen,
  _Out_opt_ ULONG* pcbWritten)
{
    nDataLen;
    pcbWritten;

    if (pvData == NULL)
    {
        return E_POINTER;
    }

    return E_NOTIMPL;
}

#pragma endregion

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
