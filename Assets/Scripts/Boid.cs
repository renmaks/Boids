using UnityEngine;

public class Boid : MonoBehaviour
{
    private Rigidbody _rigid;
    private Neighborhood _neighborhood;
    private float _velocity = 30f;
    private float _velMatching = 0.25f;
    private float _flockCentering = 0.2f;
    private float _collAvoid = 2f;
    private float _attractPull = 2f;
    private float _attractPush = 2f;
    private float _attractPushDist = 5f;


    private void Awake()
    {
        _neighborhood = GetComponent<Neighborhood>();    
        _rigid = GetComponent<Rigidbody>();

        ChooseStartPosition();
        ChooseStartVelocity();
        LookAhead();
        ChooseColorOfBoid();
    }

     private void FixedUpdate()
    {
        Vector3 vel = _rigid.velocity;

        // ПРЕДОТВРАЩЕНИЕ СТОЛКНОВЕНИЙ - избегать близких соседей
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = _neighborhood.avgClosePos;
        // Если получен вектор Vector3.zero, ничего предпринимать не надо
        if(tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= _velocity;
        }

        // СОГЛАСОВАНИЕ СКОРОСТИ - попробовать согласовать скорость с соседями
        Vector3 velAlign = _neighborhood.avgVel;
        // Согласование требуется, только если velAlign не равно Vector3.zero
        if(velAlign != Vector3.zero)
        {
            // Нас интересует только направление, поэтому нормализуем скорость
            velAlign.Normalize();
            // и затем преобразуем в выбранную скорость
            velAlign *= _velocity;
        }

        // КОНЦЕНТРАЦИЯ СОСЕДЕЙ - движение в сторону центра группы соседей
        Vector3 velCenter = _neighborhood.avgPos;
        if(velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= _velocity;
        }

        // ПРИТЯЖЕНИЕ - организовать движение в сторону объекта Attractor
        Vector3 delta = Attractor.POS - pos;
        // Проверить, куда двигаться, в сторону Attractor или от него
        bool attracted = (delta.magnitude > _attractPushDist);
        Vector3 velAttract = delta.normalized * _velocity;

        // Применить все скорости
        float fdt = Time.fixedDeltaTime;
        if(velAvoid != Vector3.zero)
        {
            vel = Vector3.Lerp(vel, velAvoid, _collAvoid * fdt);
        }
        else
        {
            if(velAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, _velMatching * fdt);
            }
            if(velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, _flockCentering * fdt);
            }
            if(velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(vel, velAttract, _attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAttract, _attractPush * fdt);
                }
            }
        }

        // Установить vel в соответствии с velocity в объекте-одиночке Spawner
        vel = vel.normalized * _velocity;
        // В заключение присвоить скорость компоненту RigidBody
        _rigid.velocity = vel;
        // Повернуть птицу клювом в сторону нового направления движения
        LookAhead();
    }
    
    private void ChooseColorOfBoid()
    {
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
        {
            r.material.color = randColor;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randColor);
    }

    private void ChooseStartPosition()
    {
        pos = Random.insideUnitSphere * Spawner.SPAWN_RADIUS;
    }

    private void ChooseStartVelocity()
    {
        Vector3 vel = Random.onUnitSphere * _velocity;
        _rigid.velocity = vel;
    }

    private void LookAhead()
    {
        // Ориентировать птицу клювом в направлении полёта
        transform.LookAt(pos + _rigid.velocity);
    }

    public Rigidbody rigid
    {
        get { return _rigid; }
        private set { }
    }

    public Vector3 pos
    {
        get { return transform.position; }
        private set { }
    }
}
