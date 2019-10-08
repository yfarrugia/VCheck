using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VCheckLib
{
    public class cSegment
    {
        const int matrix_width = 20;
        const int matrix_height = 30;
        int input_image_height;
        int input_image_width;
        int top, bottom, left, right;
        int prev_right = 20;

        int character_height;
        int character_width;
        Bitmap character_image;
        bool character_present = true;
        string path = null;

        int image_start_pixel_x = 0;
        int image_start_pixel_y = 0;
        int[] line_top = new int[50];
        int[] line_bottom = new int[50];

        int number_of_lines = 0;
        int current_line = 0;
        Bitmap input_image;

        Size averageCharacterSize = new Size();

        bool line_present = true;
        int[] sample_pixel_x = new int[20];
        int[] sample_pixel_y = new int[30];
        Color[,] character_image_pixel = new Color[600, 800];
        string output_string;

        //List<Bitmap> imgs = new List<Bitmap>();
        Dictionary<string, Bitmap> kvpSegImgs = new Dictionary<string, Bitmap>();
        int x = 1;

        //detecting individual character symbols charcter resolution 10 by 15 
        //Line and character boundary      
        public Dictionary<string, Bitmap> start_Segmentation(Bitmap bmpImg, string saveDirectory, int av_width, int av_height)
        {
            try
            {
                path = saveDirectory;
                input_image = new Bitmap(bmpImg);
                averageCharacterSize.Height = av_height;
                averageCharacterSize.Width = av_width;
                input_image_height = input_image.Height;
                input_image_width = input_image.Width;
                right = 1;
                image_start_pixel_x = 0;
                image_start_pixel_y = 0;
                identify_lines();
                current_line = 0;
                character_present = true;

                while (character_present)
                    //detect_next_character();
                    get_next_character();

                return kvpSegImgs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //identify character lines
        public void identify_lines()
        {
            try
            {
                int y = image_start_pixel_y;
                int x = image_start_pixel_x;
                bool no_black_pixel;
                int line_number = 0;
                line_present = true;
                while (line_present)
                {
                    x = image_start_pixel_x;
                    while (Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=255, G=255, B=255]")
                    {
                        x++;
                        if (x == input_image_width)
                        {
                            x = image_start_pixel_x;
                            y++;
                        }
                        if (y >= input_image_height)
                        {
                            line_present = false;
                            break;
                        }
                    }
                    if (line_present)
                    {
                        line_top[line_number] = y;
                        no_black_pixel = false;
                        while (no_black_pixel == false)
                        {
                            y++;
                            no_black_pixel = true;
                            for (x = image_start_pixel_x; x < input_image_width; x++)
                                if ((Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=0, G=0, B=0]"))
                                    no_black_pixel = false;
                        }
                        line_bottom[line_number] = y - 1;
                        line_number++;
                    }
                }
                number_of_lines = line_number;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void get_next_character()
        {
            try
            {
                image_start_pixel_x = right + 2;
                image_start_pixel_y = line_top[current_line];
                analyze_image();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void analyze_image()
        {
            try
            {
                int analyzed_line = current_line;
                get_character_bounds();
                if (character_present)
                {
                    map_character_image_pixel_matrix();
                    create_character_image();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void get_character_bounds()
        {
            try
            {
                //form related
                int x = image_start_pixel_x;
                int y = image_start_pixel_y;
                bool no_black_pixel = false;
                if (y <= input_image_height && x <= input_image_width)
                {
                    while (Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=255, G=255, B=255]")
                    {
                        x++;
                        if (x == input_image_width)
                        {
                            x = image_start_pixel_x;
                            y++;
                        }
                        if (y >= line_bottom[current_line])
                        {
                            character_present = false;
                            break;
                        }
                    }
                    if (character_present)
                    {
                        top = y;

                        x = image_start_pixel_x; y = image_start_pixel_y;
                        while (Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=255, G=255, B=255]")
                        {
                            y++;
                            if (y == line_bottom[current_line])
                            {
                                y = image_start_pixel_y;
                                x++;
                            }
                            if (x > input_image_width)
                                break;
                        }
                        if (x < input_image_width)
                            left = x;

                        no_black_pixel = true;
                        y = line_bottom[current_line] + 2;
                        while (no_black_pixel == true)
                        {
                            y--;
                            for (x = image_start_pixel_x; x < input_image_width; x++)
                                if ((Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=0, G=0, B=0]"))
                                    no_black_pixel = false;
                        }
                        bottom = y;

                        no_black_pixel = false;
                        x = left + 10;
                        while (no_black_pixel == false)
                        {
                            x++;
                            no_black_pixel = true;
                            for (y = image_start_pixel_y; y < line_bottom[current_line]; y++)
                                if ((Convert.ToString(input_image.GetPixel(x, y)) == "Color [A=255, R=0, G=0, B=0]"))
                                    no_black_pixel = false;
                        }
                        right = x - 1;
                        top = confirm_top();
                        bottom = confirm_bottom();

                        character_height = bottom - top + 1;
                        character_width = right - left + 1;
                        confirm_dimensions();
                        if (left - prev_right >= 20)
                            output_string = output_string + " ";

                        prev_right = right;

                    }
                    else if (current_line < number_of_lines - 1)
                    {
                        current_line++;
                        image_start_pixel_y = line_top[current_line];
                        image_start_pixel_x = 0;
                        prev_right = 20;
                        //output_string = output_string + "\n";
                        character_present = true;
                        get_character_bounds();
                    }
                }
                else
                    character_present = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int confirm_top()
        {
            try
            {
                int local_top = top;
                for (int j = top; j <= bottom; j++)
                    for (int i = left; i <= right; i++)
                        if (Convert.ToString(input_image.GetPixel(i, j)) == "Color [A=255, R=0, G=0, B=0]")
                        {
                            local_top = j;
                            return local_top;
                        }
                return local_top;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int confirm_bottom()
        {
            try
            {
                int local_bottom = bottom;
                for (int j = bottom; j >= 0; j--)
                    for (int i = left; i <= right; i++)
                        if (Convert.ToString(input_image.GetPixel(i, j)) != "Color [A=255, R=255, G=255, B=255]")
                        {
                            local_bottom = j;
                            return local_bottom;
                        }
                return local_bottom;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //set resolution to 20:30
        public void confirm_dimensions()
        {
            try
            {
                if (character_width < 20)
                {
                    left = left - 5; right = right + 5;
                }
                if (character_height < 30)
                {
                    top = top - 15; bottom = bottom + 15;
                }
                character_height = bottom - top + 1;
                character_width = right - left + 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void pick_sampling_pixels()
        {
            try
            {
                //form related
                int step = (int)(character_height / matrix_height);
                if (step < 1) step = 1;

                sample_pixel_y[0] = 0;
                sample_pixel_y[29] = character_height - 1;
                sample_pixel_y[19] = (int)(2 * sample_pixel_y[29] / 3);
                sample_pixel_y[9] = (int)(sample_pixel_y[29] / 3);

                sample_pixel_y[4] = (int)(sample_pixel_y[9] / 2);
                sample_pixel_y[5] = sample_pixel_y[4] + step;
                sample_pixel_y[2] = (int)(sample_pixel_y[4] / 2);
                sample_pixel_y[3] = sample_pixel_y[2] + step;
                sample_pixel_y[1] = sample_pixel_y[0] + step;
                sample_pixel_y[6] = sample_pixel_y[1] + sample_pixel_y[5];
                sample_pixel_y[7] = sample_pixel_y[2] + sample_pixel_y[5];
                sample_pixel_y[8] = sample_pixel_y[3] + sample_pixel_y[5];
                for (int i = 10; i < 19; i++)
                    sample_pixel_y[i] = sample_pixel_y[i - 10] + sample_pixel_y[9];
                for (int i = 20; i < 29; i++)
                    sample_pixel_y[i] = sample_pixel_y[i - 20] + sample_pixel_y[19];

                step = (int)(character_width / matrix_width);
                if (step < 1) step = 1;

                sample_pixel_x[0] = 0;
                sample_pixel_x[19] = character_width - 1;
                sample_pixel_x[9] = (int)(sample_pixel_x[19] / 2);

                sample_pixel_x[4] = (int)(sample_pixel_x[9] / 2);
                sample_pixel_x[5] = sample_pixel_x[4] + step;
                sample_pixel_x[2] = (int)(sample_pixel_x[4] / 2);
                sample_pixel_x[3] = sample_pixel_x[2] + step;
                sample_pixel_x[1] = sample_pixel_x[0] + step;
                sample_pixel_x[6] = sample_pixel_x[1] + sample_pixel_x[5];
                sample_pixel_x[7] = sample_pixel_x[2] + sample_pixel_x[5];
                sample_pixel_x[8] = sample_pixel_x[3] + sample_pixel_x[5];
                for (int i = 10; i < 19; i++)
                    sample_pixel_x[i] = sample_pixel_x[i - 10] + sample_pixel_x[9];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void map_character_image_pixel_matrix()
        {
            try
            {
                for (int j = 0; j < character_height; j++)
                    for (int i = 0; i < character_width; i++)
                        character_image_pixel[i, j] = input_image.GetPixel(i + left, j + top);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void create_character_image()
        {
            try
            {
                character_image = new System.Drawing.Bitmap(character_width, character_height);
                for (int j = 0; j < character_height; j++)
                    for (int i = 0; i < character_width; i++)
                        character_image.SetPixel(i, j, character_image_pixel[i, j]);

                character_image = VCheckLib.cImageProcessing.resizeImage(character_image, averageCharacterSize);

                string dir = path + x + ".bmp";
                character_image.Save(System.Web.HttpContext.Current.Server.MapPath(dir));
                kvpSegImgs.Add(dir, character_image);
                x++;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
