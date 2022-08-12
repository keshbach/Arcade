/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#pragma once

class ImageStream : public IStream
{
private:
    ULONG m_ulRefCount;

    LPCVOID m_pvData;
    DWORD m_dwDataLen;

    ULARGE_INTEGER m_Position;

public:
	ImageStream(_In_ LPCVOID pvData,
                _In_ DWORD dwDataLen);
    virtual ~ImageStream();

private:
	ImageStream();
	ImageStream(const ImageStream&);
    ImageStream& operator = (const ImageStream&);

// IUnknown
public:
    virtual ULONG STDMETHODCALLTYPE AddRef();
    virtual ULONG STDMETHODCALLTYPE Release();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** ppv);

// IStream
public:
    virtual HRESULT STDMETHODCALLTYPE Seek(_In_ LARGE_INTEGER nAmountMove,
                                           _In_ DWORD dwOrigin,
                                           _Out_opt_ ULARGE_INTEGER* pnNewPosition);
    virtual HRESULT STDMETHODCALLTYPE SetSize(ULARGE_INTEGER nNewSize);
    virtual HRESULT STDMETHODCALLTYPE CopyTo(_In_ IStream* pStream,
                                             _In_ ULARGE_INTEGER nAmount,
                                             _Out_opt_ ULARGE_INTEGER* pcbRead,
                                             _Out_opt_ ULARGE_INTEGER* pcbWritten);
    virtual HRESULT STDMETHODCALLTYPE Commit(_In_ DWORD dwCommitFlags);
    virtual HRESULT STDMETHODCALLTYPE Revert(void);
    virtual HRESULT STDMETHODCALLTYPE LockRegion(_In_ ULARGE_INTEGER nOffset,
                                                 _In_ ULARGE_INTEGER nAmount,
                                                 _In_ DWORD dwLockType);
    virtual HRESULT STDMETHODCALLTYPE UnlockRegion(_In_ ULARGE_INTEGER nOffset,
                                                   _In_ ULARGE_INTEGER nAmount,
                                                   _In_ DWORD dwLockType);
    virtual HRESULT STDMETHODCALLTYPE Stat(__RPC__out STATSTG* pStatStorage,
                                           _In_ DWORD dwStatFlag);
    virtual HRESULT STDMETHODCALLTYPE Clone(__RPC__deref_out_opt IStream** ppStream);

// ISequentialStream
public:
    virtual HRESULT STDMETHODCALLTYPE Read(_Out_writes_bytes_to_(nDataLen, *pcbRead) void* pvData,
                                           _In_ ULONG nDataLen,
                                           _Out_opt_ ULONG* pcbRead);
    virtual HRESULT STDMETHODCALLTYPE Write(_In_reads_bytes_(nDataLen) const void* pvData,
                                            _In_ ULONG nDataLen,
                                            _Out_opt_ ULONG* pcbWritten);
};

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2019-2022 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
