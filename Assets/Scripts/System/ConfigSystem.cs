using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Config;
using Event;
using QFramework;
using UIScripts;
using UnityEngine;

namespace System
{
    public interface IConfigSystem : ISystem
    {
        string CurrentLang { get; set; }

        bool InitFinish { get; set; }

        string GetText(int key);

        void RegisterText(MutiLanguageText t);
        void UnregisterText(MutiLanguageText t);
    }

    public class ConfigSystem : AbstractSystem, IConfigSystem
    {
        private Config.Config tableData;
        private Dictionary<int, string> currentLanguages = new Dictionary<int, string>();
        private string currentLanguage = "CN";
        private List<MutiLanguageText> allTexts = new List<MutiLanguageText>();

        protected override void OnInit()
        {
            //this.GetModel<ISettingsModel>().Language
            this.RegisterEvent<ChangeSettingEvent>((e) =>
            {
                SetLanguage(currentLanguage);
            });

            DebugTool.LogWithHexColor("OnInit");
            //��ʱ���ã�������Ҫ��,��������ϵͳ��̬������Ϸ���ԡ���Wildness
            LoadConfig("config.bin",LoadConfigComplete);
        
        
        }

        #region ���ر������ã�������Ҫ��������ַ���,�����г�ʼ�����ݷ���һ�𡪡�Wildness

        public void LoadConfig(string cfgName, Action<Config.Config> onLoaded)
        {
            string configPath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Configs"), cfgName);
            //DebugTool.LogWithHexColor(CoroutineController.Instance);
            if (CoroutineController.Instance != null)
            {
                CoroutineController.Instance.StartCoroutine(LoadConfigByWWW(configPath, onLoaded));
            }        
        }

        private void LoadConfigComplete(Config.Config cfg)
        {
            //DebugTool.LogWithHexColor("LoadConfigComplete");
            // init Data =INDEX(B5:S5,MATCH(MAX(LEN(B5:R5)),LEN(B5:S5),0))
            tableData = cfg;
            SetLanguage(currentLanguage);
        }

        IEnumerator LoadConfigByWWW(string filePath, Action<Config.Config> OnLoaded)
        {
            //DebugTool.LogWithHexColor("LoadConfigByWWW");
            var req = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return req.SendWebRequest();
            var stream = new MemoryStream(req.downloadHandler.data);
            var cfg = Stream2Config(stream);
            stream.Close();
            OnLoaded.Invoke(cfg);
        }

        Config.Config Stream2Config(Stream stream)
        {
            stream.Position = 0;
            var rdr = new DataReader(stream);
            var cfg = new Config.Config();
            var res = rdr.ReadHeader(cfg.GetBuildID());
            if (res != FileState.OK)
            {
                DebugTool.Error("combine file crack!");
                return null;
            }
            Config.Config.Deserialize(cfg, rdr);
            return cfg;
        }
        #endregion

        void SetTableData()
        {

        }

        void SetLanguage(string id)
        {
            currentLanguage = id;
            currentLanguages.Clear();
            for (int i = 0; i < tableData.LanguageData.Count; i++)
            {
                LanguageDataDefine temp = tableData.LanguageData[i];
                FieldInfo slot = temp.GetType().GetField(currentLanguage);
                string s = (string)slot.GetValue(temp);
                currentLanguages.Add(tableData.LanguageData[i].ID, s);
            }
            RefreshAllText();
        }

        void RefreshAllText()
        {
            MutiLanguageText[] arr = allTexts.ToArray();
            foreach (MutiLanguageText i in arr)
            {
                if (i == null)
                {
                    allTexts.Remove(i);
                }
                else
                {
                    i.Reset();
                }
            }
        }

        public string CurrentLang { get; set; }
        public bool InitFinish { get; set; }

        public string GetText(int key)
        {
            if (currentLanguages.ContainsKey(key))
            {
                return currentLanguages[key];
            }
            else
            {
                Debug.LogError("û��key�� [ " + key + "]   |�ķ���");
                return "";
            }

        }

        public void RegisterText(MutiLanguageText t)
        {
            allTexts.Add(t);
            t.Reset();
        }

        public void UnregisterText(MutiLanguageText t)
        {
            allTexts.Remove(t);
        }


    }
}