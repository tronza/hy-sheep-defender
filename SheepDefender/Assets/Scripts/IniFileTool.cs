using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

/**
 * Copyright 2013-2014 Mika Hämäläinen, Jannis Seemann
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
    public class IniFileTool
    {
        public Encoding characterEncoding = Encoding.UTF8;
        private String file = "";
        public Exception lastError;
        private List<string> inisContents = new List<string>();


        public IniFileTool(String iniFile, bool load)
        {
            initializeIniTool(iniFile, load);
        }

        public IniFileTool(String iniFile)
        {
            initializeIniTool(iniFile, true);
        }

        public IniFileTool()
        {
            file = "default.ini";
        }

        private void initializeIniTool(String iniFile, bool load)
        {
            file = iniFile;
            if (load == true)
            {
                loadIniFile(iniFile);
            }
            else
            {
                this.inisContents.Clear();
            }
        }

        public void loadIniFile(String iniTiedosto)
        {
            try
            {
                this.inisContents.Clear();
                StreamReader lataa = new StreamReader(iniTiedosto, characterEncoding);
                String linja;
                while ((linja = lataa.ReadLine()) != null)
                {
                    if (!linja.Equals(""))//tyhjiä rivejä ei ladata
                    {
                        this.inisContents.Add(linja);
                    }
                }
                this.file = iniTiedosto;
                this.lastError = null;
            }
            catch (Exception e)
            {
                this.file = "";
                this.lastError = e;
            }
        }


        public void saveIniFile() {
        try {
            StreamWriter tallenna = new StreamWriter(file, false, characterEncoding);

            foreach (String rivi in inisContents) {
                tallenna.WriteLine(rivi);
            }
            tallenna.Close();
        } catch (Exception e) {
            lastError = e;
        }
    }

        public void saveIniAs(String filePath)
        {
            file = filePath;
            saveIniFile();
        }


        public String getValue(String iniGroup, String iniKey, String defaultValue)
        {
            int kohta = inisContents.IndexOf("[" + iniGroup + "]");
            String vastaus = defaultValue;
            if (kohta == -1)
            {
                vastaus = defaultValue;
            }
            else
            {
                String haku = iniKey + "=";
                while (kohta < inisContents.Count - 1)
                {
                    kohta = kohta + 1;
                    String teksti = inisContents[kohta];
                    if (teksti.StartsWith(haku + "\""))
                    {
                        try
                        {
                            String tulos = teksti.Substring(haku.Length + 1,  (teksti.Length - 1)-(haku.Length + 1));

                            vastaus = tulos;
                        }
                        catch (Exception e)
                        {
                            vastaus = "\"";
 
                        }
                        break;
                    }
                    else if (teksti.StartsWith(haku))
                    {
                        String tulos = teksti.Substring(haku.Length, teksti.Length - haku.Length);
                        vastaus = tulos;
                        break;
                    }
                    else if (teksti.StartsWith("["))
                    {
                        break;
                    }
                }

            }
            vastaus = vastaus.Replace("LINE$", "\n");
            vastaus = vastaus.Replace("LINE_RN$", "\r\n");
            vastaus = vastaus.Replace("LINE_R$", "\r");
            return vastaus;
        }

        public int getValue(String ryhma, String jasen, int oletusArvo) {
            try
            {
                int arvo = Convert.ToInt32(getValue(ryhma, jasen, "virhe"));
                return arvo;
            }
            catch (Exception e) {
                return oletusArvo;
            }
        }
    

        public int getNumberOfGroups()
        {
            int ryhmii = 0;
            for (int i = 0; i < inisContents.Count; i++)
            {
                if (inisContents[i].StartsWith("["))
                {
                    ryhmii = ryhmii + 1;
                }
            }
            return ryhmii;
        }


        public String getGroupByIndex(int indeksi)
        {
            int ryhmii = -1;
            String vastaus = "";
            for (int i = 0; i < inisContents.Count; i++)
            {
                if (inisContents[i].StartsWith("["))
                {
                    ryhmii = ryhmii + 1;
                    if (ryhmii == indeksi)
                    {
                        vastaus = inisContents[i].Substring(1, inisContents[i].Length - 1 -1);
                        break;
                    }
                }
            }
            return vastaus;
        }


        private int groupsLine(String ryhma)
        {
            String eti = "[" + ryhma + "]";
            int vastaus = -1;
            for (int i = 0; i < inisContents.Count; i++)
            {
                if (inisContents[i]==eti)
                {
                    vastaus = i;
                    break;
                }
            }
            return vastaus;
        }


        private int keysLine(int ryhmaLinja, String jasen)
        {
            String haku = jasen + "=";
            int vastaus = -1;
            while (ryhmaLinja < inisContents.Count - 1)
            {
                ryhmaLinja = ryhmaLinja + 1;
                String teksti = inisContents[ryhmaLinja];
                if (teksti.StartsWith(haku))
                {
                    vastaus = ryhmaLinja;
                    break;
                }
                else if (teksti.StartsWith("["))
                {
                    break;
                }
            }
            return vastaus;
        }


        public void setValue(String iniGroup, String iniKey, String iniValue)
        {
            iniValue = iniValue.Replace("\r\n", "LINE_RN$");
            iniValue = iniValue.Replace("\n", "LINE$");
            iniValue = iniValue.Replace("\r", "LINE_R$");
            int ryhmal = groupsLine(iniGroup);
            if (ryhmal == -1)
            {
                inisContents.Add("[" + iniGroup + "]");
                inisContents.Add(iniKey + "=" + iniValue);
            }
            else
            {
                int jasenl = keysLine(ryhmal, iniKey);
                if (jasenl != -1)
                {
                    inisContents[jasenl] = iniKey + "=" + iniValue;
                }
                else
                {
                    inisContents.Insert(ryhmal + 1, iniKey + "=" + iniValue);
                }
            }
        }

 

        public int getNumberOfKeys(String iniGroup)
        {
            int ryhmanSij = groupsLine(iniGroup);
            if (ryhmanSij == -1)
            {
                return 0;
            }
            int jasenMaara = 0;
            for (int i = ryhmanSij + 1; i < inisContents.Count; i++)
            {
                if (!inisContents[i].StartsWith("["))
                {
                    jasenMaara++;
                }
                else
                {
                    break;
                }
            }
            return jasenMaara;
        }


        public String getKeyByIndex(String iniGroup, int keyIndex, String defaultValue)
        {
            int jasenMaara = getNumberOfKeys(iniGroup);
            if ((jasenMaara - 1) < keyIndex)
            {
                return defaultValue;
            }
            int ryhmanSij = groupsLine(iniGroup);
            int arvonSij = ryhmanSij + keyIndex + 1;
            try
            {
                String jasen = inisContents[arvonSij];
                int onKohta = jasen.IndexOf("=");
                jasen = jasen.Substring(0, onKohta);
                return jasen;
            }
            catch (Exception e)
            {
                return defaultValue;
            }
        }


        public String getValueByIndex(String iniGroup, int keyIndex, String defaultValue)
        {
            String jasen = getKeyByIndex(iniGroup, keyIndex, "");
            if (jasen=="")
            {
                return defaultValue;
            }
            return getValue(iniGroup, jasen, defaultValue);
        }
    }

