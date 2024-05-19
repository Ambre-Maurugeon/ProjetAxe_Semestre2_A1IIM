using UnityEngine;

public class WallDetector : MonoBehaviour
{
    [Header("Wall Detection")]
    [SerializeField] private Transform[] _wallDetectionPointsLeft;
    [SerializeField] private Transform[] _wallDetectionPointsRigth;
    [SerializeField] private float _detectionLength = 0.05f;
    [SerializeField] private LayerMask _wallLayerMask;

    public static float orientDetection { get; private set; }


    public bool DetectWallNearByRight()
    {
        foreach (Transform wallDetectionPoint in _wallDetectionPointsRigth)
        {

            RaycastHit2D hitRight = Physics2D.Raycast(
                     wallDetectionPoint.position, //origine
                     Vector2.right,            //direction
                     _detectionLength,
                     _wallLayerMask
                 );

            if (hitRight.collider != null)
            {
                Debug.Log("Contact de WallDetector droit");

                return true;
            }
        }
        return false;
    }
    public bool DetectWallNearByLeft()
    {
        foreach (Transform wallDetectionPoint in _wallDetectionPointsLeft)
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(
                wallDetectionPoint.position, // origine
                Vector2.left,                // direction 
                _detectionLength,
                _wallLayerMask
            );

            if (hitLeft.collider != null)
            {
                Debug.Log("Contact de WallDetector Left");
                return true;
            }
        }
        return false;

    }
}
