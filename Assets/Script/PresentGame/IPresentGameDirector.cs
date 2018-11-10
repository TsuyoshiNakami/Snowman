using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPresentGameDirector {
    void GameUpdate(float timeLimit);
    void PresentEmitUpdate();
    void OnTimerEnd();
}
