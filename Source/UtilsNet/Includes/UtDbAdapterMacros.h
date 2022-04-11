/***************************************************************************/
/*  Copyright (C) 2022-2022 Kevin Eshbach                                  */
/***************************************************************************/

#if !defined(UtDbAdapterMacros_H)
#define UtDbAdapterMacros_H

#define MDatabaseAdapterReadDictionarySetting(settingsdict, databaseregkey, regkey, settingname, defaultvalue) \
    settingsdict->Add(settingname, ReadSetting(regkey, databaseregkey, settingname, defaultvalue));

#define MDatabaseAdapterReadEncryptedDictionarySetting(settingsdict, databaseregkey, regkey, settingname, defaultvalue) \
    { \
        System::String^ _Value = ReadSetting(regkey, databaseregkey, settingname, defaultvalue); \
        if (!System::String::IsNullOrWhiteSpace(_Value)) \
        { \
            _Value = DecryptString(_Value); \
            if (_Value == nullptr) \
            { \
                _Value = L""; \
            } \
        } \
        settingsdict->Add(settingname, _Value); \
    }

#define MDatabaseAdapterVerifyWriteDictionarySetting(settingsdict, settingname, settingtype, errormessage) \
    if (!settingsdict->ContainsKey(settingname)) \
    { \
        errormessage = System::String::Format("\"{0}\" setting is missing", settingname); \
        return false; \
    } \
    { \
        System::Object^ _Object = settingsdict[settingname]; \
        if (_Object->GetType() != settingtype::typeid) \
        { \
            errormessage = System::String::Format("\"{0}\" setting is not of the type \"{1}\"", settingname, settingtype::typeid->ToString()); \
            return false; \
        } \
    }

#define MDatabaseAdapterWriteDictionarySetting(settingsdict, databaseregkey, regkey, settingname, errormessage) \
    { \
        System::Object^ _Object = settingsdict[settingname]; \
        if (_Object->GetType() == System::String::typeid) \
        { \
            if (!WriteSetting(regkey, databaseregkey, settingname, (System::String^)_Object)) \
            { \
                errormessage = System::String::Format("\"{0}\" setting could not be saved", settingname); \
                return false; \
            } \
        } \
        else if (_Object->GetType() == System::UInt16::typeid) \
        { \
            if (!WriteSetting(regkey, databaseregkey, settingname, (System::UInt16)_Object)) \
            { \
                errormessage = System::String::Format("\"{0}\" setting could not be saved", settingname); \
                return false; \
            } \
        } \
        else \
        { \
            errormessage = System::String::Format("\"{0}\" setting is an unknown type", settingname); \
            return false; \
        } \
    }

#define MDatabaseAdapterWriteEncryptedDictionarySetting(settingsdict, databaseregkey, regkey, settingname, errormessage) \
    { \
        System::Object^ _Object = settingsdict[settingname]; \
        System::String^ _Value; \
        if (_Object->GetType() == System::String::typeid) \
        { \
            _Value = EncryptString((System::String^)_Object); \
            if (_Value == nullptr) \
            { \
                errormessage = System::String::Format("\"{0}\" setting could not be encrypted", settingname); \
                return false; \
            } \
            if (!WriteSetting(regkey, databaseregkey, settingname, _Value)) \
            { \
                errormessage = System::String::Format("\"{0}\" setting could not be saved", settingname); \
                return false; \
            } \
        } \
        else \
        { \
            errormessage = System::String::Format("\"{0}\" encrypted setting is an unknown type", settingname); \
            return false; \
        } \
    }

#endif /* end of UtDbAdapterMacros_H */

/***************************************************************************/
/*  Copyright (C) 2022-2022 Kevin Eshbach                                  */
/***************************************************************************/
