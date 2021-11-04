using UnityEngine;
using System.Collections.Generic;

// 따로 할당하지 않고 전체 순위에 따른 점수를 저장하기 위한 스크립트
// DontDestroyOnLoad를 이용해서 빈 오브젝트에 저장을 해도 되지만 static을 이용해 스크립트에 저장하였습니다.
public class SaveData : MonoBehaviour
{
    // 오브젝트 형식으로 데이터 관리하는 Collections: ArrayList, Queue, Stack, Hashtable 등
    // 데이터 형식을 일반화하여 사용하는 Collections.Generic: List<T>, Dictionary<T>, Queue<T>, Stack<T> 등
    public static List<int> Score = new List<int> () { 0, 0, 0 };

    // 값을 넣고 오름차순으로 정렬된걸 역순으로 만들고 가장 끝에 있는 값을 삭제
    public static void ChangeRanking(int score)
    {
        Score.Add(score);
        Score.Sort();
        Score.Reverse();
        Score.RemoveAt(Score.Count - 1);
    }
}
