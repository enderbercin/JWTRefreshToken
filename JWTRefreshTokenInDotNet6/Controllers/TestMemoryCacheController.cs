using JWTRefreshToken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JWTRefreshToken.Controllers
{
    public class TestMemoryCacheController : Controller
    {
        public class CatalogController : Controller
        {
            const string cacheKey = "testMemoryCacheKey";
            private readonly IMemoryCache _memCache;
            public CatalogController(IMemoryCache memCache)
            {
                _memCache = memCache;
            }


            [HttpGet]
            public ActionResult<IEnumerable<TestMemoryCache>> Get()
            {
                //Catalog listemize 2 adet kategori set ediyoruz
                List<TestMemoryCache> testMemoryCacheList = new List<TestMemoryCache> { new TestMemoryCache { Name = "Diş Macunu", IsOpen = true }, new TestMemoryCache { Name = "Parfüm", IsOpen = true } };

                //Burada değerin belirtilen key ile cache'de kontrolünü yapıyoruz
                if (!_memCache.TryGetValue(cacheKey, out testMemoryCacheList))
                {
                    //Burada cache için belirli ayarlamaları yapıyoruz.Cache süresi,önem derecesi gibi
                    var cacheExpOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(30),
                        Priority = CacheItemPriority.Normal
                    };
                    //Bu satırda belirlediğimiz key'e göre ve ayarladığımız cache özelliklerine göre kategorilerimizi in-memory olarak cache'liyoruz.
                    _memCache.Set(cacheKey, testMemoryCacheList, cacheExpOptions);
                }
                return testMemoryCacheList;
            }

            [HttpGet]
            public ActionResult DeleteCache()
            {
                //Remove ile verilen key'e göre bulunan veriyi siliyoruz
                _memCache.Remove(cacheKey);
                return View();
            }

        }
    }
    }
