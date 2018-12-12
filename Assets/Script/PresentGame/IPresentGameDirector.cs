using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresentGameDirector {
    void HurryUp();
    void GameUpdate(float timeLimit);
    void PresentEmitUpdate();
    void OnTimerEnd();
}
