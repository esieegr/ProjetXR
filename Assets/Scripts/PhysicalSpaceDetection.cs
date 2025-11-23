using UnityEngine;
using Meta.XR.MRUtilityKit;

public class PhysicalSpaceDetection : MonoBehaviour
{
    [SerializeField] Transform tableau01;
    [SerializeField] Transform tableau02;

    [SerializeField] Transform tapis;     
    [SerializeField] Transform plafond;

    [SerializeField] Transform tasse;

    void Start()
    {
        Debug.Log("Physical Space Detection ready.");
    }

    // Fonction appelée lorsque la scène physique est détectée
    public void PlacePainting()
    {
        // Vérifier MRUK
        if (MRUK.Instance == null)
        {
            Debug.Log("MRUK non détecté !");
            return;
        }

        // Récupérer la room détectée
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        if (room == null)
        {
            Debug.Log("Aucune pièce détectée !");
            return;
        }

        // Vérifier murs
        int wallsNB = room.WallAnchors.Count;
        if (wallsNB < 2)
        {
            Debug.Log("Pas assez de murs détectés !");
            return;
        }

        // -----------------------------
        // 1️⃣ Premier mur → tableau01
        // -----------------------------
        Vector3 center01 = room.WallAnchors[0].transform.position;
        Vector3 normal01 = room.WallAnchors[0].transform.forward;
        float width01 = room.WallAnchors[0].PlaneRect.Value.width;
        float height01 = room.WallAnchors[0].PlaneRect.Value.height;

        tableau01.position = center01 + Vector3.up * 0.5f;
        tableau01.forward = normal01;

        // -----------------------------
        // 2️ Deuxième mur → tableau02
        // -----------------------------
        Vector3 center02 = room.WallAnchors[1].transform.position;
        Vector3 normal02 = room.WallAnchors[1].transform.forward;
        float width02 = room.WallAnchors[1].PlaneRect.Value.width;
        float height02 = room.WallAnchors[1].PlaneRect.Value.height;

        tableau02.position = center02 + Vector3.up * 0.5f;
        tableau02.forward = normal02;

        // -----------------------------
        // 3️ Tapis → sol (FloorAnchor)
        // -----------------------------
        tapis.position = room.FloorAnchor.transform.position;

        // -----------------------------
        // 4️⃣ Panneau plafond → CeilingAnchor
        // -----------------------------
        plafond.position = room.CeilingAnchor.transform.position;

        var listOfAnchors = room.Anchors;
        for (int i = 0; i < listOfAnchors.Count; i++)
        {
            if (listOfAnchors[i].Label == MRUKAnchor.SceneLabels.TABLE)
            {
                tasse.position = listOfAnchors[i].transform.position;
            }
        }
            



        Debug.Log("PlacePainting() exécutée : murs + sol + plafond OK !");
    }
}
