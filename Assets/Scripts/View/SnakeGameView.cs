using Ji2Core.Core;
using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class SnakeGameView : MonoBehaviour
    {
        [SerializeField] private Head head;
        [SerializeField] private SpriteSnakeViewConfig snakeViewConfig;
        [SerializeField] private SnakeFoodAnimationConfig snakeFoodAnimationConfig;
        [SerializeField] private GameObject oddCell;
        [SerializeField] private RectTransform fieldImage;


        private SpritesSnakeView _spriteSpritesSnakeView;
        private FoodContainerView _foodContainerView;

        private Context _context;

        public ISnakeView SpritesSnakeView => _spriteSpritesSnakeView;
        public FoodContainerView FoodContainerView => _foodContainerView;
        public Head Head => head;


        public void Initialize(Pool<SnakePartView> partsPool, Pool<FoodView> foodPool, Vector2Int fieldSize,
            PositionProvider positionProvider)
        {
            _foodContainerView = new FoodContainerView(foodPool, positionProvider);
            _spriteSpritesSnakeView = new SpritesSnakeView(partsPool, positionProvider, snakeViewConfig,
                snakeFoodAnimationConfig, head);

            fieldImage.sizeDelta = positionProvider.fieldImageSize;

            for (int i = 0; i < fieldSize.x; i++)
            {
                for (int j = 0; j < fieldSize.y; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        var pos = positionProvider.GetPoint(new Vector2Int(i, j));
                        var cell = Instantiate(oddCell, pos, Quaternion.identity, transform);
                        cell.transform.localScale =
                            new Vector3(positionProvider._cellSize, positionProvider._cellSize, 1);
                    }
                }
            }
        }

        private void Awake()
        {
            _context = Context.GetInstance();
            _context.Register(this);
        }

        private void OnDestroy()
        {
            _foodContainerView?.Destroy();
            _spriteSpritesSnakeView?.Destroy();

            _context.Unregister(this);
        }
    }
}