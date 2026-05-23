using System;
using Infernum.FPS.Core.Animation;
using Infernum.FPS.Enemy.Config;

namespace Infernum.FPS.Enemy
{
    public interface IEnemyAnimationService : ISpriteAnimationService
    {
        void BindConfig(EnemyConfig config);
        void PlayIdle();
        void PlayMove();
        void PlayAttack(Action onComplete = null);
        void PlayDead(Action onComplete = null);
        void Hide();
    }
}
