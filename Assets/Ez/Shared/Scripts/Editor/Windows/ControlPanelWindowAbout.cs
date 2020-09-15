// Copyright (c) 2016 - 2018 Ez Entertainment SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using Ez.Internal;
using QuickEditor;
using QuickEngine.Core;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace Ez
{
    public partial class ControlPanelWindow : QWindow
    {
        EzModuleVersion _defineSymbolsModuleVersion;
        EzModuleVersion DefineSymbolsModuleVersion
        {
            get
            {
#if !EZ_DEFINE_SYMBOLS
                return null;
#endif

#pragma warning disable CS0162 // Unreachable code detected
                if(_defineSymbolsModuleVersion == null)
                {
                    _defineSymbolsModuleVersion = Q.GetResource<EzModuleVersion>(EZT.RESOURCES_PATH_DEFINE_SYMBOLS_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);

                    if(_defineSymbolsModuleVersion == null)
                    {
                        _defineSymbolsModuleVersion = Q.CreateAsset<EzModuleVersion>(EZT.RELATIVE_PATH_DEFINE_SYMBOLS_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);
                    }
                }
                return _defineSymbolsModuleVersion;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        EzModuleVersion _dataManagerModuleVersion;
        EzModuleVersion DataManagerModuleVersion
        {
            get
            {
#if !EZ_DATA_MANAGER
                return null;
#endif

#pragma warning disable CS0162 // Unreachable code detected
                if(_dataManagerModuleVersion == null)
                {
                    _dataManagerModuleVersion = Q.GetResource<EzModuleVersion>(EZT.RESOURCES_PATH_DATA_MANAGER_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);

                    if(_dataManagerModuleVersion == null)
                    {
                        _dataManagerModuleVersion = Q.CreateAsset<EzModuleVersion>(EZT.RELATIVE_PATH_DATA_MANAGER_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);
                    }
                }
                return _dataManagerModuleVersion;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        EzModuleVersion _bindModuleVersion;
        EzModuleVersion BindModuleVersion
        {
            get
            {
#if !EZ_BIND
                return null;
#endif

#pragma warning disable CS0162 // Unreachable code detected
                if(_bindModuleVersion == null)
                {
                    _bindModuleVersion = Q.GetResource<EzModuleVersion>(EZT.RESOURCES_PATH_BIND_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);

                    if(_bindModuleVersion == null)
                    {
                        _bindModuleVersion = Q.CreateAsset<EzModuleVersion>(EZT.RELATIVE_PATH_BIND_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);
                    }
                }
                return _bindModuleVersion;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        EzModuleVersion _poolyModuleVersion;
        EzModuleVersion PoolyModuleVersion
        {
            get
            {
#if !EZ_POOLY
                return null;
#endif

#pragma warning disable CS0162 // Unreachable code detected
                if(_poolyModuleVersion == null)
                {
                    _poolyModuleVersion = Q.GetResource<EzModuleVersion>(EZT.RESOURCES_PATH_POOLY_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);

                    if(_poolyModuleVersion == null)
                    {
                        _poolyModuleVersion = Q.CreateAsset<EzModuleVersion>(EZT.RELATIVE_PATH_POOLY_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);
                    }
                }
                return _poolyModuleVersion;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        EzModuleVersion _adsModuleVersion;
        EzModuleVersion AdsModuleVersion
        {
            get
            {
#if !EZ_ADS
                return null;
#endif

#pragma warning disable CS0162 // Unreachable code detected
                if(_adsModuleVersion == null)
                {
                    _adsModuleVersion = Q.GetResource<EzModuleVersion>(EZT.RESOURCES_PATH_ADS_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);

                    if(_adsModuleVersion == null)
                    {
                        _adsModuleVersion = Q.CreateAsset<EzModuleVersion>(EZT.RELATIVE_PATH_ADS_VERSION, EzModuleVersion.MODULE_VERSION_FILENAME);
                    }
                }
                return _adsModuleVersion;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        AnimBool showDefineSymbolsReleaseNotes;
        AnimBool showDataManagerReleaseNotes;
        AnimBool showBindReleaseNotes;
        AnimBool showPoolyReleaseNotes;
        AnimBool showAdsReleaseNotes;

        AnimBool editModuleAnimBool;

        Page selectedModuleToEdit = Page.None;

        void InitAbout()
        {
            InitAboutAnimBools();

            selectedModuleToEdit = Page.None;
        }

        void InitAboutAnimBools()
        {
            showDefineSymbolsReleaseNotes = new AnimBool(false, Repaint);
            showDataManagerReleaseNotes = new AnimBool(false, Repaint);
            showBindReleaseNotes = new AnimBool(false, Repaint);
            showPoolyReleaseNotes = new AnimBool(false, Repaint);
            showAdsReleaseNotes = new AnimBool(false, Repaint);

            editModuleAnimBool = new AnimBool(false, Repaint);
        }

        void DrawAbout()
        {
            DrawPageHeader("ABOUT", QColors.Green, "Module Versions & Release Notes", QUI.IsProSkin ? QColors.UnityLight : QColors.UnityMild, EZResources.IconAbout);
            QUI.Space(6);

            editModuleAnimBool.target = selectedModuleToEdit != Page.None;

            DrawSourceData(WindowSettings.CurrentPageContentWidth + 16);

            QUI.Space(SPACE_16);

            DrawSourcePrereleaseCheck(WindowSettings.CurrentPageContentWidth + 16);
        }


        void DrawSourceData(float width)
        {
            QUI.Space(SPACE_16);
            DrawModuleVersion("EZ DEFINE SYMBOLS", Page.DefineSymbols, DefineSymbolsModuleVersion, showDefineSymbolsReleaseNotes, width);
            QUI.Space(SPACE_8);
            DrawModuleVersion("EZ DATA MANAGER", Page.DataManager, DataManagerModuleVersion, showDataManagerReleaseNotes, width);
            QUI.Space(SPACE_8);
            DrawModuleVersion("EZ BIND", Page.DataBind, BindModuleVersion, showBindReleaseNotes, width);
            QUI.Space(SPACE_8);
            DrawModuleVersion("POOLY", Page.Pooly, PoolyModuleVersion, showPoolyReleaseNotes, width);
            QUI.Space(SPACE_8);
            DrawModuleVersion("EZ ADS", Page.Ads, AdsModuleVersion, showAdsReleaseNotes, width);
            QUI.Space(SPACE_8);
        }

        void DrawModuleVersion(string moduleName, Page targetModule, EzModuleVersion emv, AnimBool show, float width)
        {
            if(QUI.GhostBar(moduleName + (emv == null ? " has not been installed!" : (" version " + emv.versionNumber)), emv == null ? QColors.Color.Gray : QColors.Color.Green, show, width, 24))
            {
                if(emv == null)
                {
                    show.target = false;
                    WindowSettings.currentPage = targetModule;
                }
                else
                {
                    show.target = !show.target;
                    if(selectedModuleToEdit == targetModule)
                    {
                        selectedModuleToEdit = Page.None;
                        editModuleAnimBool.target = false;
                    }
                }
            }

            if(emv == null)
            {
                return;
            }

            if(QUI.BeginFadeGroup(show.faded))
            {
                QUI.BeginVertical(width);
                {

                    QUI.Space(-9);

                    QUI.BeginHorizontal(width);
                    {
                        EditorGUILayout.LabelField(emv.releaseNotes, QStyles.GetInfoMessageMessageStyle(Style.InfoMessage.Help));
                    }
                    QUI.EndHorizontal();

#if EZ_SOURCE
#pragma warning disable CS0162 // Unreachable code detected

                    QUI.Space(13);
                    QUI.Space(8 * (selectedModuleToEdit == targetModule ? editModuleAnimBool.faded : 0));
                    QUI.BeginHorizontal(width);
                    {
                        QUI.FlexibleSpace();
                        if(QUI.GhostButton("Edit Version", QColors.Color.Gray, 100, 16, selectedModuleToEdit == targetModule)
                            || QUI.DetectKeyUp(Event.current, KeyCode.Escape))
                        {
                            if(selectedModuleToEdit == targetModule)
                            {
                                selectedModuleToEdit = Page.None;
                                editModuleAnimBool.target = false;
                            }
                            else
                            {
                                selectedModuleToEdit = targetModule;
                                editModuleAnimBool.target = true;
                            }
                        }

                        if(selectedModuleToEdit == targetModule && editModuleAnimBool.faded > 0.4f)
                        {
                            QUI.Space(SPACE_4 * editModuleAnimBool.faded);

                            if(QUI.GhostButton("Save Changes", QColors.Color.Green, 100 * editModuleAnimBool.faded, 16))
                            {
                                QUI.SetDirty(emv);
                                AssetDatabase.SaveAssets();
                                selectedModuleToEdit = Page.None;
                                editModuleAnimBool.value = false;
                            }
                        }
                    }
                    QUI.EndHorizontal();


                    if(selectedModuleToEdit == targetModule)
                    {
                        if(QUI.BeginFadeGroup(editModuleAnimBool.faded))
                        {
                            QUI.BeginVertical(width);
                            {
                                QUI.BeginHorizontal(width);
                                {
                                    EditorGUILayout.LabelField("version", QStyles.GetStyle(QStyles.GetStyleName(Style.Text.Small)), GUILayout.Width(80));
                                    QUI.Space(SPACE_2);
                                    EditorGUILayout.LabelField("release notes", QStyles.GetStyle(QStyles.GetStyleName(Style.Text.Small)));
                                }
                                QUI.EndHorizontal();

                                QUI.Space(-SPACE_4);

                                QUI.BeginHorizontal(width);
                                {
                                    emv.versionNumber = EditorGUILayout.TextField(emv.versionNumber, GUILayout.Width(80));
                                    QUI.Space(SPACE_2);
                                    emv.releaseNotes = EditorGUILayout.TextArea(emv.releaseNotes);
                                }
                                QUI.EndHorizontal();

                                QUI.Space(SPACE_16);
                            }
                            QUI.EndVertical();
                        }
                        QUI.EndFadeGroup();
                    }
#endif
                    QUI.Space(9 + 16);
                }
                QUI.EndVertical();
            }
            QUI.EndFadeGroup();


#pragma warning restore CS0162 // Unreachable code detected
        }

        void DrawSourcePrereleaseCheck(float width)
        {
#if !EZ_SOURCE
            return;
#endif
#pragma warning disable CS0162 // Unreachable code detected

            QLabel.text = "Pre-release Check";
            QLabel.style = Style.Text.Normal;
            QUI.Label(QLabel);

            QUI.Space(SPACE_2);

            QLabel.text = "Control Panel Window";
            QLabel.style = Style.Text.Small;
            QUI.Label(QLabel);

            DrawDeleteFile(EZT.RELATIVE_PATH_CONTROL_PANEL_WINDOW_SETTINGS, "ControlPanelWindowSettings", width, ".asset");
            QUI.Space(SPACE_4);

#if EZ_DEFINE_SYMBOLS
            QLabel.text = "Define Symbols";
            QLabel.style = Style.Text.Small;
            QUI.Label(QLabel);

            DrawFixFolderButton("Presets", EZT.RELATIVE_PATH_DEFINE_SYMBOLS_PRESETS, width);
            QUI.Space(SPACE_4);
#endif

#if EZ_DATA_MANAGER
            QLabel.text = "Data Manager";
            QLabel.style = Style.Text.Small;
            QUI.Label(QLabel);

            DrawFixFolderButton("Settings", EZT.RELATIVE_PATH_DATA_MANAGER_SETTINGS, width);
            QUI.Space(SPACE_2);
            DrawFixFolderButton("Keys", EZT.RELATIVE_PATH_DATA_MANAGER_KEYS, width);
            QUI.Space(SPACE_2);
            DrawDeleteFile(EZT.RELATIVE_PATH_DATA_MANAGER + "Scripts/", "EzDataManagerVars", width);
            QUI.Space(SPACE_4);
#endif

#if EZ_POOLY
            QLabel.text = "Pooly";
            QLabel.style = Style.Text.Small;
            QUI.Label(QLabel);

            DrawFixFolderButton("Settings", EZT.RELATIVE_PATH_POOLY_SETTINGS, width);
            QUI.Space(SPACE_2);
            DrawFixFolderButton("Statistics", EZT.RELATIVE_PATH_POOLY_STATISTICS, width);
            QUI.Space(SPACE_4);
#endif

#if EZ_ADS
            QLabel.text = "Ads";
            QLabel.style = Style.Text.Small;
            QUI.Label(QLabel);
            DrawFixFolderButton("Settings", EZT.RELATIVE_PATH_ADS_SETTINGS, width);
#endif

#pragma warning restore CS0162 // Unreachable code detected
        }

        void DrawFixFolderButton(string folderName, string relativePath, float width)
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(relativePath.Substring(0, relativePath.Length - 1), typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "Fix Folder: " + folderName;
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 24, 18))
                {
                    DeleteAllFilesUnderTargetPath(relativePath);
                }

                if(CheckThatFolderExists(relativePath))
                {

                }
                else if(CheckThatFolderIsEmpty(relativePath))
                {

                }
                else
                {
                    QUI.Space(SPACE_4);
                    QUI.BeginVertical(16, 16);
                    {
                        QUI.Space(2);
                        QUI.DrawTexture(QResources.iconOk.texture, 14, 14);
                    }
                    QUI.EndVertical();
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }

        bool CheckThatFolderExists(string relativePath)
        {
            if(!Directory.Exists(relativePath))
            {
                QUI.Space(SPACE_4);

                QLabel.text = "This folder does not exist. Click to create it.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconError.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }
            return false;
        }
        bool CheckThatFolderIsEmpty(string relativePath)
        {
            var fileNames = Directory.GetFiles(relativePath);

            if(fileNames.Length > 0)
            {
                QUI.Space(SPACE_4);

                QLabel.text = "This folder contains files. Click to delete them.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconWarning.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }

            return false;
        }
        void DeleteAllFilesUnderTargetPath(string relativePath)
        {
            if(!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }
            else
            {
                var fileNames = Directory.GetFiles(relativePath);

                for(int i = 0; i < fileNames.Length; i++)
                {
                    File.Delete(fileNames[i]);
                }

                Directory.Delete(relativePath);
                Directory.CreateDirectory(relativePath);
            }
            AssetDatabase.Refresh();
        }

        void DrawDeleteFile(string relativePath, string fileName, float width, string fileExtension = ".cs")
        {
            QUI.BeginHorizontal(width);
            {
                QLabel.text = "Find";
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 16, 18))
                {
                    // Load object
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(relativePath + fileName + fileExtension, typeof(UnityEngine.Object));

                    // Select the object in the project folder
                    Selection.activeObject = obj;

                    // Also flash the folder yellow to highlight it
                    EditorGUIUtility.PingObject(obj);
                }

                QUI.Space(SPACE_2);

                QLabel.text = "Delete: " + fileName;
                QLabel.style = Style.Text.Button;
                if(QUI.GhostButton(QLabel.text, QColors.Color.Gray, QLabel.x + 24, 18))
                {
                    AssetDatabase.DeleteAsset(relativePath + fileName + fileExtension);
                }

                if(CheckThatFileExists(relativePath, fileName, fileExtension))
                {

                }
                else
                {
                    QUI.Space(SPACE_4);
                    QUI.BeginVertical(16, 16);
                    {
                        QUI.Space(2);
                        QUI.DrawTexture(QResources.iconOk.texture, 14, 14);
                    }
                    QUI.EndVertical();
                }

                QUI.FlexibleSpace();
            }
            QUI.EndHorizontal();
        }
        bool CheckThatFileExists(string relativePath, string fileName, string fileExtension)
        {
            if(File.Exists(relativePath + fileName + fileExtension))
            {
                QUI.Space(SPACE_4);

                QLabel.text = "File exists. Click to delete it.";
                QLabel.style = Style.Text.Help;

                QUI.BeginVertical(16, 16);
                {
                    QUI.Space(2);
                    QUI.DrawTexture(QResources.iconError.texture, 14, 14);
                }
                QUI.EndVertical();

                QUI.Space(-SPACE_2);

                QUI.Label(QLabel);

                return true;
            }

            return false;
        }

        void DrawEditableLinkButtons(List<LinkButtonData> list, float width)
        {
#if !EZ_SOURCE
            return;
#endif
#pragma warning disable CS0162 // Unreachable code detected
            QUI.Space(SPACE_2);

            QUI.DrawIconBar("Edit Link Buttons", QResources.iconInfo, QColors.Color.Blue, IconPosition.Right, width, 20);
            if(list != null && list.Count > 0)
            {
                QUI.BeginChangeCheck();
                for(int i = 0; i < list.Count; i++)
                {
                    QUI.BeginHorizontal(width);
                    {
                        list[i].text = EditorGUILayout.DelayedTextField(list[i].text, GUILayout.Width(width * 0.4f));
                        QUI.Space(SPACE_2);
                        list[i].url = EditorGUILayout.DelayedTextField(list[i].url);
                        QUI.Space(SPACE_2);
                        list[i].linkButton = (Style.LinkButton)EditorGUILayout.EnumPopup(list[i].linkButton, GUILayout.Width(width * 0.1f));
                        QUI.Space(SPACE_2);
                        if(QUI.ButtonMinus())
                        {
                            list.RemoveAt(i);
                            QUI.SetDirty(EzSourceData.Instance);
                            AssetDatabase.SaveAssets();
                            QUI.ExitGUI();
                        }
                        QUI.Space(10);
                    }
                    QUI.EndHorizontal();
                }


                if(QUI.EndChangeCheck())
                {
                    QUI.SetDirty(EzSourceData.Instance);
                    AssetDatabase.SaveAssets();
                }
            }
            else
            {
                QUI.BeginHorizontal(width);
                {
                    QLabel.text = "There are no link buttons defined... This is not right!";
                    QLabel.style = Style.Text.Help;
                    QUI.Label(QLabel);
                }
                QUI.EndHorizontal();
            }

            QUI.BeginHorizontal(width);
            {
                QUI.FlexibleSpace();
                if(QUI.ButtonPlus())
                {
                    list.Add(new LinkButtonData());
                    QUI.SetDirty(EzSourceData.Instance);
                    AssetDatabase.SaveAssets();
                }
                QUI.Space(6);
            }
            QUI.EndHorizontal();
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
