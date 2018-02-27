using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//на каждую ячейку назначен свой контроллер
public class Cell_Controller : MonoBehaviour {


	public enum CellStatusEnum { empty, O, X }
	public CellStatusEnum CellStatus;  //статус ячейки  (пустой, нолик, крестик)



	public void OnMouseDown()
	{
		//отмечаем ячейку крестиком 
		if (MainController.NextMove==MainController.nextMoveEnum.X)
		{
			this.gameObject.GetComponentsInChildren<MarkerX>(true)[0].gameObject.SetActive(true);
			this.gameObject.GetComponentsInChildren<MarkerO>(true)[0].gameObject.SetActive(false);
			CellStatus = CellStatusEnum.X;
		}
		else
		//отмечаем ячейку ноликом
		if (MainController.NextMove == MainController.nextMoveEnum.O)
		{
			this.gameObject.GetComponentsInChildren<MarkerX>(true)[0].gameObject.SetActive(false);
			this.gameObject.GetComponentsInChildren<MarkerO>(true)[0].gameObject.SetActive(true);
			CellStatus = CellStatusEnum.O;
		}


		this.gameObject.GetComponent<Collider>().enabled = false;   //отключаем клики по ячейке

		MainController.MarkedCells++;				//увеличиваем количество отмеченных ячеек	 
		MainController.Instance.SetNextMove();		//меняем значение следующего хода
		MainController.Instance.CheckResult();		//проверка на конец игры

	}

 
}
