using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class MainController : MonoBehaviour {

	public static MainController Instance;  //синглтон
	public static int MarkedCells=0;        //количество отмеченных ячеек
	public enum nextMoveEnum { O, X };
	public static nextMoveEnum NextMove;	//значение следующего хода.  крестик или нолик

	public float CellSize;  //размер игрового поля
		
	public Text NextMoveInfo; 
	public Text WinnerInfo; 
	public Button BtnNewGame; 
	
	public GameObject StandartCellGO;// стандартная ячейка
	public float CellsOffset; // расстояние между ячейками
	
	public GameObject ParentGO;   //используется как родительский объект для ячеек
	public float ParentGOScale;		//необходимо для корректного масштабирования при назначении дочерним объектом
	private GameObject[] Cells;  //массив ячеек



	private void Awake()
	{
		if (Instance==null)
			Instance = this;
	}


	void Start()
	{
		
		BtnNewGame.onClick.AddListener(StartNewGame);
		BuildGrid();
		StartNewGame();
	}

	void StartNewGame()
	{
		MarkedCells = 0;
		NextMove = nextMoveEnum.X;
		NextMoveInfo.text = "следующий ход: X";
		BtnNewGame.gameObject.SetActive(false);
		WinnerInfo.text = "";
		
		//Reset ячеек
		foreach (GameObject cell in Cells)
		{
			cell.gameObject.GetComponentsInChildren<MarkerX>(true)[0].gameObject.SetActive(false);   
			cell.gameObject.GetComponentsInChildren<MarkerO>(true)[0].gameObject.SetActive(false);   
			cell.GetComponent<Cell_Controller>().CellStatus = Cell_Controller.CellStatusEnum.empty;
			cell.GetComponent<Collider>().enabled = true;
		}
	}

	
	
	void BuildGrid() // создание игрового поля
	{

				float posX = -CellSize * 3 / 2 - CellSize / 2;  //значение  position.x для первой ячейки 
				float posY = Mathf.Abs(posX);                   //значение  position.y для первой ячейки 
				float Xreset = posX;                            //исходной значение  position.x для первых ячеек ряда

				int i = 0;
				
				Cells = new GameObject[3 * 3];

				for (int y = 0; y < 3; y++)     //размножение ячеек и их распределение по игровому полю
				{
					posY -= CellSize;
					for (int x = 0; x < 3; x++)
					{
						posX += CellSize;
						Cells[i] = Instantiate(StandartCellGO) as GameObject;
						Cells[i].transform.SetParent(ParentGO.transform);
						Cells[i].transform.localScale = new Vector3((CellSize -CellsOffset)/ ParentGOScale, Cells[i].transform.localScale.y, (CellSize - CellsOffset)/ ParentGOScale);
						Cells[i].gameObject.name = "Cell_ID_" + i;
						Cells[i].transform.position= new Vector3(posX, 0, posY);
						i++;
					}
					posX = Xreset;
				}
			StandartCellGO.gameObject.SetActive(false);

	}

	 


	public void SetNextMove()
	{
		if (NextMove == nextMoveEnum.O)
		{
			NextMove = nextMoveEnum.X;
			NextMoveInfo.text = "следующий ход: X";
		}
		else
		{

			NextMove = nextMoveEnum.O;
			NextMoveInfo.text = "следующий ход: O";
		}

	}
 

	void GameEnd(Cell_Controller.CellStatusEnum winnersState)
	{
		BtnNewGame.gameObject.SetActive(true);

		WinnerInfo.text=(winnersState ==Cell_Controller.CellStatusEnum.O)? "Победили нолики": "Победили крестики";

		NextMoveInfo.text = "";
		foreach (GameObject cell in Cells)
		{
			cell.GetComponent<Collider>().enabled = false;
			cell.GetComponent<Cell_Controller>().CellStatus = Cell_Controller.CellStatusEnum.empty;
		}
	}

	void GameEndWithDrawResult()
	{
		BtnNewGame.gameObject.SetActive(true);
		WinnerInfo.text = "Ничья!";
		NextMoveInfo.text = "";
		foreach (GameObject cell in Cells)
		{
			cell.GetComponent<Collider>().enabled = false;
			cell.GetComponent<Cell_Controller>().CellStatus = Cell_Controller.CellStatusEnum.empty;
		}
	}


	public void CheckResult()
	{

		//проверка по горизонтали
		for (int i = 0; i < 3; i++)    
		{
			if (
				(Cells[i * 3 + 0].GetComponent<Cell_Controller>().CellStatus == Cells[i * 3 + 1].GetComponent<Cell_Controller>().CellStatus) &&
				(Cells[i * 3 + 0].GetComponent<Cell_Controller>().CellStatus == Cells[i * 3 + 2].GetComponent<Cell_Controller>().CellStatus)&&
				(Cells[i * 3 + 0].GetComponent<Cell_Controller>().CellStatus != Cell_Controller.CellStatusEnum.empty))
				{
					GameEnd(Cells[i * 3 + 0].GetComponent<Cell_Controller>().CellStatus);
					return;
				}
		}


		//проверка по горизонтали
		for (int j = 0; j < 3; j++)    
		{
			if (
				(Cells[j ].GetComponent<Cell_Controller>().CellStatus == Cells[j + 3].GetComponent<Cell_Controller>().CellStatus) &&
				(Cells[j ].GetComponent<Cell_Controller>().CellStatus == Cells[j + 6].GetComponent<Cell_Controller>().CellStatus))
				if (Cells[j].GetComponent<Cell_Controller>().CellStatus != Cell_Controller.CellStatusEnum.empty)

				{
					GameEnd(Cells[j].GetComponent<Cell_Controller>().CellStatus);
					return;

				}
		}



		//проверка по диагонали 1
		if ((Cells[0].GetComponent<Cell_Controller>().CellStatus== Cells[4].GetComponent<Cell_Controller>().CellStatus)&&
			(Cells[0].GetComponent<Cell_Controller>().CellStatus == Cells[8].GetComponent<Cell_Controller>().CellStatus))
			if (Cells[0].GetComponent<Cell_Controller>().CellStatus != Cell_Controller.CellStatusEnum.empty)

			{
				GameEnd(Cells[0].GetComponent<Cell_Controller>().CellStatus);
				return;

			}



		//проверка по диагонали 1
		if ((Cells[2].GetComponent<Cell_Controller>().CellStatus == Cells[4].GetComponent<Cell_Controller>().CellStatus) &&
			(Cells[2].GetComponent<Cell_Controller>().CellStatus == Cells[6].GetComponent<Cell_Controller>().CellStatus))
			if (Cells[2].GetComponent<Cell_Controller>().CellStatus!=Cell_Controller.CellStatusEnum.empty)
			{

				GameEnd(Cells[2].GetComponent<Cell_Controller>().CellStatus);
				return;

			}


		if (MarkedCells>=9)
		{
			GameEndWithDrawResult();
		}



	}

	 
 

	 
}
