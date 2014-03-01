using UnityEngine;



public class UGExcelDataManager
{

	/**
 * 	请将获得的ExcelData保存,避免重复读表
 * 
 */

	public static void  TryGet(string fileName,out ExcelData ed)
	{
		ed = null;

		UGExcelDataSource[] eds = GameObject.Find("UGLocalData").GetComponents<UGExcelDataSource>();

		for(int i = 0;i < eds.Length;i++)
		{

			if(eds[i].FileName == fileName)
			{
				ed = eds[i].data;
				break;
			}
		}
	}
}
