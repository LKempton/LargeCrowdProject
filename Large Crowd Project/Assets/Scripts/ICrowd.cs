
namespace CrowdAI
{
    public interface ICrowd
    {
        void SetState(string State);
        string GetState();
      
        void StartAnimations(float delay);
        void StartAnimation();
        void StopAnimation();
        bool LoopAnimation { get; set; }

        void DisableRenderer();

        
      
        // possibly use a way of using a mix of global and local animations
    }

  public enum CrowdFormation
    {
        CIRCLE,SQUARE, RING
    }
}
