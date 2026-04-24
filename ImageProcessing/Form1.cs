namespace ImageProcessing;

public partial class Form1 : Form
{
    private Button btnLoad;
    private Button btnProcess;
    private PictureBox pbOriginal, pbFilter1, pbFilter2, pbFilter3, pbFilter4;
    private Bitmap currentImage;

    public Form1()
    {
        InitializeCustomUI();
    }

    private void InitializeCustomUI()
    {
        this.Text = "Wielowątkowe Przetwarzanie Obrazu";
        this.Size = new Size(1050, 650);

        btnLoad = new Button { Text = "Load Image", Location = new Point(10, 10), Size = new Size(120, 40) };
        btnLoad.Click += BtnLoad_Click;

        btnProcess = new Button { Text = "Parallel Processing", Location = new Point(140, 10), Size = new Size(150, 40), Enabled = false };
        btnProcess.Click += BtnProcess_Click;

        this.Controls.Add(btnLoad);
        this.Controls.Add(btnProcess);

        // Ustawienie PictureBox'ów
        int pbW = 300, pbH = 250;
        pbOriginal = CreatePB(10, 60, pbW, pbH);
        pbFilter1 = CreatePB(330, 60, pbW, pbH);
        pbFilter2 = CreatePB(650, 60, pbW, pbH);
        pbFilter3 = CreatePB(330, 330, pbW, pbH);
        pbFilter4 = CreatePB(650, 330, pbW, pbH);
    }

    private PictureBox CreatePB(int x, int y, int w, int h)
    {
        var pb = new PictureBox
        {
            Location = new Point(x, y),
            Size = new Size(w, h),
            SizeMode = PictureBoxSizeMode.Zoom,
            BorderStyle = BorderStyle.FixedSingle
        };
        this.Controls.Add(pb);
        return pb;
    }

    private void BtnLoad_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                currentImage = new Bitmap(ofd.FileName);
                pbOriginal.Image = currentImage;
                
                // Czyszczenie starych wyników
                pbFilter1.Image = null; pbFilter2.Image = null; 
                pbFilter3.Image = null; pbFilter4.Image = null;
                
                btnProcess.Enabled = true;
            }
        }
    }

    private async void BtnProcess_Click(object sender, EventArgs e)
    {
        if (currentImage == null) return;

        btnProcess.Enabled = false;
        btnLoad.Enabled = false;

        // Tasks (z puli wątków).
        // Każde zadanie wykonuje się równolegle w osobnym wątku roboczym
        Task<Bitmap> task1 = Task.Run(() => ImageFilters.ApplyGrayscale(currentImage));
        Task<Bitmap> task2 = Task.Run(() => ImageFilters.ApplyNegative(currentImage));
        Task<Bitmap> task3 = Task.Run(() => ImageFilters.ApplyThreshold(currentImage));
        Task<Bitmap> task4 = Task.Run(() => ImageFilters.ApplyRedFilter(currentImage));

        // Czekamy asynchronicznie, aż wszystkie 4 wątki zakończą pracę
        await Task.WhenAll(task1, task2, task3, task4);

        // Zwrócenie obrazów do GUI (bezpieczne, bo await wraca do wątku głównego)
        pbFilter1.Image = task1.Result;
        pbFilter2.Image = task2.Result;
        pbFilter3.Image = task3.Result;
        pbFilter4.Image = task4.Result;

        btnProcess.Enabled = true;
        btnLoad.Enabled = true;
    }
}