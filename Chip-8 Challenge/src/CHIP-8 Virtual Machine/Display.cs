namespace CHIP_8_Virtual_Machine;

public class Display
{
    private bool[,] _pixels;

    public bool this[int x, int y] 
    { 
        get => _pixels[x, y]; 
        set { _pixels[x, y] = value; OnDisplayUpdated?.Invoke(this, _pixels); }
    }

    public event EventHandler<bool[,]> OnDisplayUpdated;

    public Display()
    {
        _pixels = new bool[64, 32];
    }

    public bool DisplayChar(int x, int y, char c)
    {
        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F'))
        {
            int charIndex = c - '0';
            byte[] sprite = SystemFont.Bytes[charIndex..(charIndex + 5)];
            return DisplaySprite(x, y, sprite);
        }
        else
        {
            throw new ArgumentOutOfRangeException("Character must be between 0 and F");
        }
    }

    public bool DisplaySprite(int x, int y, byte[] spriteBytes)
    {
        bool pixelErased = false;
        for (int i = 0; i < spriteBytes.Length; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                byte spriteByte = spriteBytes[i];
                int localX = (x + j) % 64; // wraps the X coordinate (super-smart thinking from Garry)
                int localY = (y + i) % 32; // wraps the Y coordinate

                bool currentPixelState = _pixels[localX, localY];
                bool shouldDisplay = (spriteByte & (0x1 << j)) != 0; // checks if bit j is set
                if (currentPixelState && shouldDisplay)
                {
                    pixelErased = true;
                    shouldDisplay = false;
                }

                _pixels[localX, localY] = shouldDisplay;
            }       
        }

        OnDisplayUpdated?.Invoke(this, _pixels);    

        return pixelErased;
    }
}
