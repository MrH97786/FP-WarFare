public abstract class EnemyBaseState
{
    public Enemy enemy;
    public StateController stateController; 

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}
