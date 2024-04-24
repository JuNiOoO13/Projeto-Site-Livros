using System.Drawing;

namespace CantoLiterário___projeto_LivroOnline.Services
{
    public class CreateImageService : ICreateImageService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CreateImageService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void GenerateImage(string name)
        {
            string primeiraLetra = name[0].ToString();
            string caminhoPastaImagens = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens","UsersImg");


            string caminhoCompleto = Path.Combine(caminhoPastaImagens, name);


            Bitmap imagem = new Bitmap(200, 200);
            using (Graphics graphics = Graphics.FromImage(imagem))
            {
                Random random = new Random();
                Color corAleatoria = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                SolidBrush brush = new SolidBrush(corAleatoria);
                graphics.FillRectangle(brush, 0, 0, 200, 200);

                Font fonte = new Font("Arial", 100);
                brush = new SolidBrush(Color.Black);

                SizeF tamanhoTexto = graphics.MeasureString(primeiraLetra.ToString(), fonte);
                float x = (200 - tamanhoTexto.Width) / 2;
                float y = (200 - tamanhoTexto.Height) / 2;

                graphics.DrawString(primeiraLetra, fonte, brush, x, y);
            }

            imagem.Save(caminhoCompleto, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
            
}

