using System.Threading.Tasks;

namespace CsmlWeb.Resources {
    public interface IResource {
        /// <summary>
        /// Должен быть уникален в течении сессии генерации.
        /// Предполагается что файлы не изменяются в течении сессии => если ресурс конструируется из файла,
        /// то в роли ключа можно использовать имя файла + имя класса + параметры.
        /// </summary>
        public string Key { get; }

        public string Source { get; }
        public Task GenerateAsync();
    }
}