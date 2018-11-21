using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleManager :Singleton<BattleManager>
{
    private List<HeroEntity> mEntityList = new List<HeroEntity>();
    private List<int> mRemoveList = new List<int>();
    public List<HeroEntity> entities { get {
            
            for(int i = mEntityList.Count -1; i >=0; --i)
            {
                if(mEntityList[i] == null)
                {
                    mEntityList.RemoveAt(i);
                }
            }
            return mEntityList;
        }
    }
    private List<EffectEntity> mEffects = new List<EffectEntity>();
    public HeroLogic logic { get; private set; }
    public BattleData battleData { get; private set; }
    public BattleStatus status { get; private set; }

    /// <summary>
    /// 创建队列
    /// </summary>
    private Queue<HeroData> mCreateQueue = new Queue<HeroData>();

    private Camera mUICamera;
    public Camera uiCamera
    {
        get
        {
            if(mUICamera==null)
            {
                if( UICamera.first)
                {
                    mUICamera = UICamera.first.GetComponent<Camera>();
                }
            }
            return mUICamera;
        }
    }
    private Camera mCamera;
    public Camera mainCamera
    {
        get
        {
            if (mCamera == null)
            {
                if (Camera.main)
                {
                    mCamera = Camera.main;
                }
            }
            return mCamera;
        }
    }

    public HeroTopShow topShow { get; private set; }
    public BattleShow show { get; private set; }

   /// private ObjectInstance assetPoint;
    private BattlePoints mPoints;

    public void Start()
    {
        AIConfig.Init();

        topShow = new HeroTopShow();
        show = new BattleShow();

        logic = new HeroLogic();
      
        status = BattleStatus.Prepare;
       


        string assetBundleName = "assets/assetbundle/model/scene_models/battlescene/points.prefab";

        /*
        AssetCache.LoadAssetAsync<GameObject>(assetBundleName, assetBundleName, (asset) =>
        {
            assetPoint = ObjectInstancePool.instance.GetObjectInstance(asset);
            if (assetPoint.m_gameObject != null)
            {
                assetPoint.m_gameObject.SetActive(true);
                assetPoint.m_gameObject.transform.SetParent(null);
                assetPoint.m_gameObject.transform.localPosition = Vector3.zero;
                assetPoint.m_gameObject.transform.localScale = Vector3.one;
                assetPoint.m_gameObject.transform.localRotation = Quaternion.identity;

                mPoints = assetPoint.m_gameObject.GetComponent<BattlePoints>();

                if (battleData == null)
                {
                    TRACE.Log("BattleManager TestData");
                     InitBattleTest();
                }
                for (int i = 0; i < battleData.attackList.Count; ++i)
                {
                    var heroData = battleData.attackList[i];

                    heroData.x = mPoints.HeroPoints[0].position.x;
                    heroData.y = 0;
                    heroData.z = mPoints.HeroPoints[0].position.z;

                    GetHeroConfig(ref heroData);


                    mCreateQueue.Enqueue(heroData);
                }

                for (int i = 0; i < battleData.defenseList.Count; ++i)
                {
                    var heroData = battleData.defenseList[i];
                    heroData.x = mPoints.CannonPoints[i].position.x;
                    heroData.y = 0;
                    heroData.z = mPoints.CannonPoints[i].position.z;
                    GetHeroConfig(ref heroData);
                    mCreateQueue.Enqueue(heroData);
                }
            }     
        });
        */
    }

    public void Init(BattleData data)
    {
        battleData = data;
      
    }

    public void InitBattleTest()
    {
        battleData = new BattleData();
        /*
        HeroData data1 = ObjectPool.GetInstance<HeroData>();
        data1.id = 1;
        data1.camp =  HeroCamp.Attack;
        data1.IID = 1;
        data1.x = 10;
        data1.z = 10;
        GetHeroConfig(ref data1);
        battleData.attackList.Add(data1);
        mCreateQueue.Enqueue(data1);
        */
        //英雄
        /*
        for (int i = 0; i < 5; ++i)
        {
            HeroData data = ObjectPool.GetInstance<HeroData>();
            data.id = 2 + i;
            data.IID = i % 4 + 1;
            data.camp = HeroCamp.Attack;

         
            GetHeroConfig(ref data);

            battleData.attackList.Add(data);
        }
        */
        /*
        //炮塔
        for (int i = 0; i < 8; ++i)
        {
            HeroData data = ObjectPool.GetInstance<HeroData>();
            data.id = 100 + i;
            data.IID = 201 + i%5;
            data.camp = HeroCamp.Defense;
           
            data.rotation = 90;
            data.scale = 3;
            GetHeroConfig(ref data);

            battleData.defenseList.Add(data);
        }
        */
    }

    private void GetHeroConfig(ref HeroData data)
    {
        if(data == null)
        {
            return;
        }
       
    }

 

    public HeroEntity CreateHero(HeroData data, Transform parent = null) 
    {
        if(GetEntity(data.id)!=null)
        {
            Debug.LogError("重复ID");
            return null;
        }

        HeroEntity entity =  new HeroEntity();
       
        if(entity == null)
        {
            return entity;
        }

        entity.Init(data);

        mEntityList.Add(entity);
        if(parent)
        {
            entity.gameObject.transform.SetParent(parent);
        }

        return entity;
    }

    public EffectEntity CreateEffect(Type type, Transform parent = null) 
    {
        EffectEntity entity = ObjectPool.GetInstance<EffectEntity>(type);

        mEffects.Add(entity);
        if (parent)
        {
            entity.gameObject.transform.SetParent(parent);
        }

        return entity;
    }

    public HeroEntity GetEntity(int id)
    {
        for (int i = 0; i < entities.Count; ++i)
        {
            if (entities[i] != null)
            {
                if (entities[i].data.id == id)
                {
                    return entities[i];
                }
            }
        }
        return null;
    }

    public void RemoveEntity(int id)
    {
        mRemoveList.Add(id);
         
    }

    public int GetEntityCount(HeroCamp camp)
    {
        int count = 0;
        for (int i = entities.Count - 1; i >= 0; --i)
        {
            var entity = entities[i];
            if (entity!=null && entity.data.camp ==camp)
            {
                count++;
            }
        }
        return count;
    }

    public void RemoveEffect(EffectEntity entity)
    {
        for (int i = mEffects.Count - 1; i >= 0; --i)
        {
            var effect = mEffects[i];
            if (effect == null||effect == entity)
            {
                entity.Recycle();
                mEffects.RemoveAt(i);
            }
        }
    }

    public void Update(float deltaTime)
    {
        while(mRemoveList.Count>0)
        {
            int id = mRemoveList[0];
            mRemoveList.RemoveAt(0);
            for (int i = mEntityList.Count - 1; i >= 0; --i)
            {
                var entity = mEntityList[i];
                if (entity == null)
                {
                    mEntityList.RemoveAt(i);
                }
                else
                {
                    if (entity.data.id == id)
                    {
                        entity.Recycle();
                        mEntityList.RemoveAt(i);
                    }
                }
            }
            topShow.Remove(id);
        }

        if (status == BattleStatus.None)
        {
            return;
        }


        if (mCreateQueue.Count > 0)
        {
            CreateHero(mCreateQueue.Dequeue(), null);
        }
        if (status == BattleStatus.Fighting)
        {
            if (battleData.BattleTime > 0)
            {
                battleData.BattleTime -= deltaTime;
                if (battleData.BattleTime < 0) battleData.BattleTime = 0;
            }

            if (battleData.BattleTime == 0)
            {
                BattleFinish();
            }
            else
            {
                CheckBattleFinish();
            }
        }

        for (int i = entities.Count - 1; i >= 0; --i)
        {
            var player = entities[i];
            /*
            if (status == BattleStatus.Fighting && logic != null)
            {
                var input = player as BTree.BTInput;
                logic.Tick(player.data.ai, ref input);
            }*/

            player.OnUpdate(deltaTime);

        }

        for (int i = mEffects.Count - 1; i >= 0; --i)
        {
            if (mEffects[i] == null)
            {
                mEffects.RemoveAt(i);
            }
            else
            {

                mEffects[i].OnUpdate(deltaTime);
            }

        }

        CheckInput();
    }


    public void LateUpdate()
    {
        if (status == BattleStatus.None)
        {
            return;
        }
        for (int i = 0; i < entities.Count; ++i)
        {
            var player = entities[i];
            Vector3 position = player.gameObject.transform.position;
            position.y += player.data.height;
            topShow.SetPosition(player.data, position);
        }
    }

    private void CheckBattleFinish()
    {
        int attackCount = GetEntityCount(HeroCamp.Attack);
        int defenseCount = GetEntityCount(HeroCamp.Defense);

    }

    private void BattleFinish()
    {
        status = BattleStatus.Finish;

    }

    private void  CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*
            if (mainCamera)
            {
                Ray tmpRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit tmpHit;
                if (Physics.Raycast(tmpRay, out tmpHit, 1000))
                {
                    var entity = GetEntity(1);
                    if (entity!=null)
                    {
                        entity.data.destination = tmpHit.point;
                        entity.data.stopDistance = HeroData.DEFAULT_STOPDISTANCE;
                    }
                }
            }
            */
            if (status == BattleStatus.Fighting)
            {
                HeroData data = ObjectPool.GetInstance<HeroData>();
                data.id = BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0);
                data.IID = 1 + (int)Time.time % 4;
                data.camp = HeroCamp.Defense;

                data.rotation = 90;
                int i = UnityEngine.Random.Range(0, mPoints.CannonPoints.Length);
                data.x = mPoints.CannonPoints[i].position.x;
                data.y = 0;
                data.z = mPoints.CannonPoints[i].position.z;
                GetHeroConfig(ref data);

                battleData.defenseList.Add(data);
                mCreateQueue.Enqueue(data);
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            var entity = GetEntity(1) ;
            if (entity != null)
            {
                entity.FindTarget();

                entity.ReleaseSkill(SkillEnum.Attack);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var entity = GetEntity(1);
            if (entity != null)
            {
                entity.FindTarget();


                entity.ReleaseSkill(SkillEnum.Skill);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(mCreateQueue.Count ==0)
            {
                status = BattleStatus.Fighting;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (status == BattleStatus.Fighting)
            {
                HeroData data1 = ObjectPool.GetInstance<HeroData>();
                data1.Clear();
                data1.id = BitConverter.ToInt32( Guid.NewGuid().ToByteArray(),0);
                data1.camp = HeroCamp.Attack;
                data1.IID =  (int)Time.time % 4 + 1;
                int i = UnityEngine.Random.Range(0, mPoints.HeroPoints.Length);
                data1.x = mPoints.HeroPoints[i].position.x;
                data1.y = 0;
                data1.z = mPoints.HeroPoints[i].position.z;
                data1.scale = 1;
                GetHeroConfig(ref data1);
                battleData.attackList.Add(data1);
                mCreateQueue.Enqueue(data1);
            }
        }
    }
    private static List<HeroEntity> scanList;
    /// <summary>
    /// 扫描视野伞形区域的所有人物
    /// </summary>
    /// <param name="position"></param>
    /// <param name="forward"></param>
    /// <param name="distance"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public List<HeroEntity> ScanView(Vector3 position, Vector3 forward, float distance, float angle)
    {
        if (scanList == null) scanList = new List<HeroEntity>();
        scanList.Clear();

        List<HeroEntity> heroList = entities;
        for (int i = 0; i < heroList.Count; ++i)
        {
            var hero = heroList[i];
            if (hero == null)
            {
                continue;
            }

            if(hero.position == position)
            {
                continue;
            }
            float d = Vector3.Distance(hero.position, position);

            if (d < distance)
            {
                //归一化
                forward.Normalize();
                //投影的xz平面
                forward = forward- Vector3.Project(forward, Vector3.up);

                Vector3 direction = (hero.position - position);

                //归一化
                direction.Normalize();
                //投影的xz平面
                direction = direction - Vector3.Project(direction, Vector3.up);
                //计算夹角
                float tmpAngle =Vector3.Angle(forward,direction);

                if (tmpAngle < angle * 0.5f)
                {
                    scanList.Add(hero);
                }
            }
        }
        return scanList;
    }
    private static NavMeshPath path = new NavMeshPath();

    public bool FindPath(Vector3 sourcePosition, Vector3 targetPosition,ref List<Vector3> paths)
    {
        path.ClearCorners();
        if (NavMesh.CalculatePath(sourcePosition, targetPosition, NavMesh.AllAreas, path))
        {
            paths.Clear();
            paths.AddRange(path.corners);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取目标targetPosition的周围distance的距离的一个没有人（怪物）站位的点，使站位不重叠（适用于攻击）
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="targetPosition"></param>
    /// <param name="distance"></param>
    /// <param name="angleOffset">角度间隔</param>
    /// <returns></returns>
    public Vector3 MoveTowardsWidthUmbrella(HeroEntity entity, Vector3 targetPosition, float distance, float angleOffset = 120)
    {
        if (entity == null)
        {
            return targetPosition;
        }
        Vector3 foundPosition = targetPosition;

        List<HeroEntity> entityList = entities;

        float currentAngle = 0;

        Vector3 direction = entity.position - targetPosition;
        direction.Normalize();
        direction = direction - Vector3.Project(direction, Vector3.up);

        Quaternion r = Quaternion.LookRotation(direction);

        int index = 0;

        bool found = false;

        while (currentAngle < 360 && angleOffset >= 15)
        {
            Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y - (index % 2 == 1 ? currentAngle : 360 - currentAngle), r.eulerAngles.z); ///求出第i个点的旋转角度

            foundPosition = targetPosition + (q * Vector3.forward) * distance;///该点的坐标

            Vector3 dir = (foundPosition - targetPosition);
            dir.Normalize();
            dir = dir - Vector3.Project(dir, Vector3.up);

            bool exsitPlayer = false;
            bool exsitSelf = false;
            for (int i = 0; i < entityList.Count; ++i)
            {
                var player = entityList[i];
                if (player == null)
                {
                    continue;
                }
                Vector3 playerPosition = player.position;
                if (playerPosition == targetPosition)
                {
                    continue;
                }

                float playerDistance = Vector3.Distance(playerPosition, targetPosition);

                bool inRange = false;

                if (playerDistance <= distance +1)
                {
                    inRange = true;
                }
                else
                {
                    playerPosition = player.data.destination;
                    playerDistance = Vector3.Distance(playerPosition, targetPosition);
                    if (playerDistance <= distance + 1)
                    {
                        inRange = true;
                    }
                }

                if (inRange)
                {
                    Vector3 forward = playerPosition - targetPosition;
                    forward.Normalize();
                    forward = forward - Vector3.Project(forward, Vector3.up);

                    float tmpAngle = Mathf.Acos(Vector3.Dot(dir, forward)) * Mathf.Rad2Deg;
                    if (tmpAngle < angleOffset * 0.5f)
                    {
                        if (entityList[i] == entity)
                        {
                            //是自己
                            exsitSelf = true;
                            continue;
                        }
                        else
                        {
                            //是别人
                            exsitPlayer = true;
                            exsitSelf = false;
                            break;
                        }
                    }
                }
            }
            //没有其他人
            if (exsitPlayer == false)
            { 
                found = true;
                break;

            }
            else
            {
                //这个范围内只有自己
                if (exsitSelf == true)
                {

                    found = true;
                    break;

                }
            }

            ++index;

            if (index % 2 == 0)
            {
                currentAngle += angleOffset;
            }

            if (currentAngle >= 360)
            {
                currentAngle = 0;
                angleOffset = angleOffset * 0.5f;
                index = 0;
            }

        }
        if (found == false)
        {
            foundPosition = targetPosition + direction * distance;
        }
        return foundPosition;
    }


    public Vector3 MoveTowardsWidthRadius(HeroEntity entity, Vector3 targetPosition, float distance, float angleOffset = 120)
    {
        if (entity == null)
        {
            return targetPosition;
        }
        Vector3 foundPosition = targetPosition;

        List<HeroEntity> entityList = entities;

       
        float currentAngle = 0;

        Vector3 direction = entity.position - targetPosition;
        direction.Normalize();
        direction = direction - Vector3.Project(direction, Vector3.up);

        Quaternion r = Quaternion.LookRotation(direction);

        int index = 0;

        bool found = false;


        while (currentAngle < 360 && angleOffset >= 15)
        {
            Quaternion q = Quaternion.Euler(r.eulerAngles.x, r.eulerAngles.y - (index % 2 == 1 ? currentAngle : 360 - currentAngle), r.eulerAngles.z); ///求出第i个点的旋转角度

            foundPosition = targetPosition + (q * Vector3.forward) * distance;///该点的坐标

            float radius = Mathf.Min(distance * Mathf.Sin(angleOffset * 0.5f * Mathf.PI / 180f), distance);

            bool exsitEntity = false;
            bool exsitSelf = false;

            for (int i = 0; i < entityList.Count; ++i)
            {
                var player = entityList[i];
                if (player == null)
                {
                    continue;
                }

                float playerDistance = Vector3.Distance(foundPosition, entityList[i].position);
                if (playerDistance < radius)
                {
                    if (entityList[i] == entity)
                    {
                        //是自己
                        exsitSelf = true;
                        continue;
                    }
                    else
                    {
                        //是别人
                        exsitEntity = true;
                        if (exsitSelf == true)
                        {
                            exsitSelf = false;
                        }
                        break;
                    }
                }


                if (Vector3.Distance(foundPosition, entityList[i].data.destination) < radius)
                {
                    exsitEntity = true;
                    if (exsitSelf == true)
                    {
                        exsitSelf = false;
                    }
                    break;
                }
            }
            //没有其他人
            if (exsitEntity == false)
            {
                found = true;
                break;
            }
            else
            {
                //这个范围内只有自己
                if (exsitSelf == true)
                {
                    found = true;
                    break;
                }
            }

            ++index;

            if (currentAngle == 0 || index % 2 == 0)
            {
                currentAngle += angleOffset;
            }

            if (currentAngle >= 360)
            {
                currentAngle = 0;
                angleOffset = angleOffset * 0.5f;
                index = 0;
            }

        }
        if (found == false)
        {
            foundPosition = targetPosition + direction * distance;
        }
        return foundPosition;
    }

    public void Destroy()
    {
       
        //if (assetPoint != null)
        //{
        //    UnityEngine.Object.Destroy(assetPoint.m_gameObject);
        //}
        if (topShow != null)
            topShow.Destroy();
        if (show != null)
            show.Destroy();
        if (logic != null)
            logic.Destroy();

      
    }
   
}

