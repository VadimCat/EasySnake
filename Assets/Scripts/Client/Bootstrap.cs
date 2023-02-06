using Client.States;
using Core.Compliments;
using Ji2.Models.Analytics;
using Ji2Core.Core;
using Ji2Core.Core.Audio;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.UserInput;
using Ji2Core.Plugins.AppMetrica;
using UI.Background;
using UnityEngine;

namespace Client
{
    public class Bootstrap : BootstrapBase
    {
        // [SerializeField] private LevelsViewDataStorageBase<> levelsStorageBase;
        
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private BackgroundService backgroundService;
        [SerializeField] private UpdateService updateService;
        [SerializeField] private TextCompliments complimentsWordsService;
        [SerializeField] private AudioService audioService;
        
        private AppSession appSession;

        private readonly Context context = Context.GetInstance();

        protected override void Start()
        {
            DontDestroyOnLoad(this);
            //TODO: Create installers where needed
            InstallCamera();
            InstallAudioService();
            InstallLevelsData();
            InstallNavigator();
            InstallInputService();
            context.Register(updateService);
            
            var sceneLoader = new SceneLoader(updateService);
            
            var analytics = new Analytics(); 
            analytics.AddLogger(new YandexMetricaLogger(AppMetrica.Instance));
            
            // var levelService = new LevelService(
            //     // levelsStorageBase, 
            //     levelViewOrigin, screenNavigator, updateService,
            //     backgroundService, context, sceneLoader, analytics);

            context.Register(sceneLoader);
            context.Register(audioService);
            // context.Register(levelService);
            context.Register(complimentsWordsService);
            
            StateMachine appStateMachine = new StateMachine(new StateFactory(context));
            
            appSession = new AppSession(appStateMachine);
            
            appSession.StateMachine.Enter<InitialState>();
        }

        private void InstallCamera()
        {
            context.Register(new CameraProvider());
        }

        private void InstallInputService()
        {
            context.Register(new InputService(updateService));
        }

        private void InstallAudioService()
        {
            audioService.Bootstrap();
            audioService.PlayMusic(AudioClipName.DefaultBackgroundMusic);
        }

        private void InstallNavigator()
        {
            screenNavigator.Bootstrap();
            context.Register(screenNavigator);
        }

        private void InstallLevelsData()
        {
            // levelsStorageBase.Bootstrap();
        }
    }
}