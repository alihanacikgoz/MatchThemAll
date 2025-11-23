using MatchThemAll.Scripts.Runtime.Data;
using UnityEngine.Events;

namespace MatchThemAll.Scripts.Runtime.Signals
{
    /// <summary>
    /// Seviye yönetimi ile ilgili olayları merkezi olarak yayınlamak için kullanılır.
    /// </summary>
    public static class LevelSignals
    {
        /// <summary>
        /// Bir seviye yüklendiğinde ve başlatılmaya hazır olduğunda tetiklenir.
        /// LevelData'yı parametre olarak gönderir, böylece diğer sistemler seviye ayarlarını alabilir.
        /// </summary>
        public static UnityAction<LevelData> onLevelInitialize;
        
        /// <summary>
        /// Seviye başarıyla başlatıldığında tetiklenir.
        /// </summary>
        public static UnityAction onLevelStart;
        
        /// <summary>
        /// Seviye başarıyla tamamlandığında tetiklenir.
        /// </summary>
        public static UnityAction onLevelSuccess;
        
        /// <summary>
        /// Seviyede başarısız olunduğunda tetiklenir.
        /// </summary>
        public static UnityAction onLevelFailed;
    }
}