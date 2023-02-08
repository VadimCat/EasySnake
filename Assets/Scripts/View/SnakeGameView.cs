using Ji2Core.Core;
using Ji2Core.Core.Pools;
using UnityEngine;

namespace Views
{
    public class SnakeGameView : MonoBehaviour
    {
        [SerializeField] private Gradient gradient;
        [SerializeField] private LineSnakeView lineSnakeView;
        [SerializeField] private TrailSnakeView trailSnakeView;

        private SpritesSnakeView _spriteSpritesSnakeView;
        private FoodContainerView _foodContainerView;
        
        private Context _context;

        public ISnakeView SpritesSnakeView => _spriteSpritesSnakeView;
        // public ISnakeView SnakeView => _snakeSnakeView;
        public FoodContainerView FoodContainerView => _foodContainerView;

        public void Initialize(Pool<SnakePartView> partsPool, Pool<FoodView> foodPool, Vector2Int initialSize)
        {
            PositionProvider positionProvider = new PositionProvider(initialSize);
            _foodContainerView = new FoodContainerView(foodPool, positionProvider);
            
            // trailSnakeView.SetDependencies(positionProvider);
            // lineSnakeView.SetDependencies(positionProvider, 5);
            
            _spriteSpritesSnakeView = new SpritesSnakeView(partsPool, positionProvider, gradient);
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
