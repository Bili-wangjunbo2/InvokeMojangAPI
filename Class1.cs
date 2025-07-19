using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Text.Json;

namespace MojangAPILibrary1
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
    public class MojangSkinInfo
    {
        public long timestamp { get; set; }
        public string profileId { get; set; }
        public string profileName { get; set; }
        public Textures textures { get; set; }
    }

    public class Textures
    {
        public Skin SKIN { get; set; }
        public Cape CAPE { get; set; }
    }

    public class Skin
    {
        public string url { get; set; }
    }

    public class Cape
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
            var url = "https://api.mojang.com/users/profiles/minecraft/" + playerName;
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            MojangProfileID profileId = JsonSerializer.Deserialize<MojangProfileID>(result);
            return $"{profileId.id}";
        }




        /// <summary>
        /// 获取玩家披风
        /// </summary>
        /// <param name="playerUuid"></param>
        /// <returns></returns>
        public async Task<string> GetSkinUrl(string playerUuid)
        {
            var client = new HttpClient();
            var url = $"https://sessionserver.mojang.com/session/minecraft/profile/{playerUuid}";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<MojangProfileValue>(json);
            string encodedValue = profile?.properties?.Find(p => p.name == "textures")?.value;

            if (string.IsNullOrEmpty(encodedValue)) return "未找到 skin 数据";

            // 1️⃣ Base64 解码
            byte[] bytes = Convert.FromBase64String(encodedValue);
            string decodedJson = Encoding.UTF8.GetString(bytes);

            // 2️⃣ 反序列化内部 JSON
            var skinInfo = JsonSerializer.Deserialize<MojangSkinInfo>(decodedJson);

            return $"皮肤 URL：{skinInfo.textures?.SKIN?.url ?? "无"}\n披风 URL：{skinInfo.textures?.CAPE?.url ?? "无"}";
        }



    }


    
}
