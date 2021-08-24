using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TDFramework;
using TDFramework.Extention;
public class ExcelHandle
{
    private string m_DataPath;
   public ExcelHandle(string dataPath)
    {
        m_DataPath = dataPath;
        LoadData();
    }

    private void LoadData()
    {
        if (m_DataPath.IsNull())
            return;
        // ��ñ�����
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(m_DataPath, ref columnNum, ref rowNum);

        //����excel�Ķ��壬�����п�ʼ��������
        for (int i = 1; i < rowNum; i++)
        {
            for (int j = 1; j < columnNum; j++)
            {
                string data=collect[i][j].ToString();

            }
        }
    }


    /// <summary>
    /// ��ȡexcel�ļ�����
    /// </summary>
    /// <param name="filePath">�ļ�·��</param>
    /// <param name="columnNum">����</param>
    /// <param name="rowNum">����</param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //Tables[0] �±�0��ʾexcel�ļ��е�һ�ű������
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }


}
