/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "DbTableColumn.h"

Common::Data::DbTableColumn::DbTableColumn()
{
}

Common::Data::DbTableColumn::DbTableColumn(
  System::String^ sColumnName,
  System::Int32 nColumnLength,
  EColumnType ColumnType)
{
    m_sColumnName = sColumnName;
    m_nColumnLength = nColumnLength;
    m_ColumnType = ColumnType;
}

Common::Data::DbTableColumn::~DbTableColumn()
{
    m_sColumnName = nullptr;
}

/////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2008-2014 Kevin Eshbach
/////////////////////////////////////////////////////////////////////////////
