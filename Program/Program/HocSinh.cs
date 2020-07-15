using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Resources
{
    public class HocSinh
    {
        public string hsID;
        public string name;
        public string classroom;
        public string year;
        public string dOB;
        public float weight;
        public float height;
        public string gender;
        public string address;
        public string dName;
        public string mName;
        public string dJob;
        public string mJob;
        public int dYOB; // dad's year of birth
        public int mYOB;
        public string phone;
        public string yearj;
        public HocSinh()
        {
        }

        public HocSinh(string hsID, string name, string classroom, string year, string dOB, float weight, float height, string gender, string address, string dName, string mName, string dJob, string mJob, int dYOB, int mYOB, string phone, string yearj)
        {
            this.hsID = hsID;
            this.name = name;
            this.classroom = classroom;
            this.year = year;
            this.dOB = dOB;
            this.weight = weight;
            this.height = height;
            this.gender = gender;
            this.address = address;
            this.dName = dName;
            this.mName = mName;
            this.dJob = dJob;
            this.mJob = mJob;
            this.dYOB = dYOB;
            this.mYOB = mYOB;
            this.phone = phone;
            this.yearj = yearj;
        }

        public HocSinh(DataRow dataRow)
        {
            this.hsID = dataRow[0].ToString();
            this.name = dataRow[1].ToString();
            this.classroom = dataRow[2].ToString();
            this.year = dataRow[3].ToString();
            this.dOB = dataRow[4].ToString();
            this.weight = float.Parse(dataRow[5].ToString());
            this.height = float.Parse(dataRow[6].ToString());
            this.gender = dataRow[7].ToString();
            this.address = dataRow[8].ToString();
            this.dName = dataRow[9].ToString();
            this.mName = dataRow[10].ToString();
            this.dJob = dataRow[11].ToString();
            this.mJob = dataRow[12].ToString();
            this.dYOB =Convert.ToInt32(dataRow[13].ToString());
            this.mYOB = Convert.ToInt32(dataRow[14].ToString());
            this.phone = dataRow[15].ToString();
            this.yearj = dataRow[16].ToString();
        }
    }
}
