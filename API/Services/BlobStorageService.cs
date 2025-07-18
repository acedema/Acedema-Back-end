using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace TuProyecto.Servicios
{
    /// <summary>
    /// Servicio para subir archivos a Azure Blob Storage.
    /// </summary>
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        /// <summary>
        /// Constructor del servicio.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión del Storage Account.</param>
        /// <param name="containerName">Nombre del contenedor Blob donde se subirán los archivos.</param>
        public BlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        /// <summary>
        /// Sube un archivo al contenedor Blob especificado.
        /// </summary>
        /// <param name="archivoStream">Flujo del archivo que se desea subir.</param>
        /// <param name="nombreArchivo">Nombre con el que se guardará el archivo en Azure Blob Storage.</param>
        /// <returns>URL del archivo subido (si el contenedor es público).</returns>
        public async Task<string> SubirArchivoAsync(Stream archivoStream, string nombreArchivo)
        {
            // Crear el cliente del servicio Blob usando la cadena de conexión
            var blobServiceClient = new BlobServiceClient(_connectionString);

            // Obtener el cliente del contenedor
            var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Crear el contenedor si no existe
            await containerClient.CreateIfNotExistsAsync();

            // Obtener el cliente del blob (archivo) con el nombre deseado
            var blobClient = containerClient.GetBlobClient(nombreArchivo);

            // Subir el archivo al blob, sobrescribiendo si ya existe
            await blobClient.UploadAsync(archivoStream, overwrite: true);

            // Retornar la URL pública del archivo subido
            return blobClient.Uri.ToString();
        }
    }
}
