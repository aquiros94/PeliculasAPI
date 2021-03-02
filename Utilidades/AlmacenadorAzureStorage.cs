using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Utilidades
{
    public class AlmacenadorAzureStorage : IAlmacenadorArchivos
    {
        private readonly string connectionString;

        public AlmacenadorAzureStorage(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            string extension, nombreArchivo;
            BlobClient blob;
            BlobContainerClient cliente;

            cliente = await AccesoContenedor(connectionString, contenedor);
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob); //Decimos que el acceso es publico
            extension = Path.GetExtension(archivo.FileName);
            nombreArchivo = $"{Guid.NewGuid()}{extension}";
            blob = cliente.GetBlobClient(nombreArchivo); //Crea un blob cliente con el archivo
            await blob.UploadAsync(archivo.OpenReadStream()); //Sube el archivo

            return blob.Uri.ToString();
        }

        private async Task<BlobContainerClient> AccesoContenedor(string connectionString, string contenedor)
        {
            BlobContainerClient cliente;

            cliente = new BlobContainerClient(connectionString, contenedor); //Conectamos azure storage con un cliente
            await cliente.CreateIfNotExistsAsync(); //creamos el contenedor si no existe

            return cliente;
        }

        public async Task BorrarArchivo(string ruta, string contenedor)
        {
            string nombreArchivo;
            BlobClient blob;
            BlobContainerClient cliente;

            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            cliente = await AccesoContenedor(connectionString, contenedor);
            nombreArchivo = Path.GetFileName(ruta);
            blob = cliente.GetBlobClient(nombreArchivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }
    }
}
