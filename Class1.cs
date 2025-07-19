using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace InvokeMojangAPI
{

    sealed class MojangProfileID
    {
        public string id { get; set; }        
    }

    sealed class MojangProfileValue
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<MojangProperty> properties { get; set; }
    }

    sealed class MojangProperty
    {
        public string name { get; set; }
        public string value { get; set; }
    }
    sealed class MojangSkinInfo
    {
        public long timestamp { get; set; }
        public string profileId { get; set; }
        public string profileName { get; set; }
        public Textures textures { get; set; }
    }

    sealed class Textures
    {
        public Skin SKIN { get; set; }
        public Cape CAPE { get; set; }
    }

    sealed class Skin
    {
        public string url { get; set; }
    }

    sealed class Cape
    {
        public string url { get; set; }
    }

    /// <summary>
    /// 解析MojangAPI类
    /// </summary>
    public class MojangAPI
    {

        /// <summary>
        /// 获取玩家UUID
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public async Task<string> GetPlayerUUID(string playerName)
        {          
            var client = new HttpClient();
            var url = $"https://api.mojang.com/users/profiles/minecraft/{playerName}";
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            MojangProfileID profileId = JsonSerializer.Deserialize<MojangProfileID>(result);
            return $"{profileId.id}";
        }




        /// <summary>
        /// 获取玩家皮肤
        /// </summary>
        /// <param name="playerUuid"></param>
        /// <returns></returns> 
        public async Task<string> GetPlayerSkin(string uuid)
        {
            var client = new HttpClient();
            string url = $"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}";
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<MojangProfileValue>(result);
            string encoded = profile?.properties?.Find(p => p.name == "textures")?.value;

            if (string.IsNullOrEmpty(encoded))
            {
                return "未找到皮肤数据";
            }                
            string decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var skinInfo = JsonSerializer.Deserialize<MojangSkinInfo>(decodedJson);

            return skinInfo?.textures?.SKIN?.url ?? "无皮肤";
        }

        /// <summary>
        /// 获取玩家披风
        /// </summary>
        /// <param name="playerUuid"></param>
        /// <returns></returns> 
        public async Task<string> GetPlayerCape(string uuid)
        {
            var client = new HttpClient();
            string url = $"https://sessionserver.mojang.com/session/minecraft/profile/{uuid}";
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<MojangProfileValue>(result);
            string encoded = profile?.properties?.Find(p => p.name == "textures")?.value;

            if (string.IsNullOrEmpty(encoded))
            {
                return "未找到披风数据";
            }
                
            string decodedJson = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var skinInfo = JsonSerializer.Deserialize<MojangSkinInfo>(decodedJson);

            return skinInfo?.textures?.CAPE?.url ?? "无披风";
        }



    }


    
}
