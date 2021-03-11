using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace WaterBox
{
    public class ActorMaster : MonoBehaviour
    {
        // 主角所在的块
        public int tilePosX;
        public int tilePosY;
        public int tilePosZ;
        public bool isGravity = true;

        //  角色控制器
        CharacterController controller;

        static Vector3 moveDirection = new Vector3();
        float m_Speed = 4;
        float jumpSpeed = 15;
        float m_Gravity = 10;
        bool m_IsEnabled = true;

        bool m_IsDownAtck = false;

        public bool Enabled 
        {
            get { return m_IsEnabled;  }
            set { m_IsEnabled = value; }
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        void Start()
        {
            EventSystem.get().registrationEvent("TestEvent", ()=> {
                UtilsCommon.log("Run ActorMaster RegistrationEvent");
            });
            controller = GetComponent<CharacterController>();
            string eventNameAdd = EventEnum.getEventEnumString(EventEnum.EventEnumType.UI_CONSOLE_STATE_CHANGED_int);
            EventSystem.get().registrationEvent<int>(eventNameAdd, onConsoleStateChanged);
        }

        /// <summary>
        /// 更新函数
        /// </summary>
        void Update()
        {
            tilePosX = (int)Math.Floor(transform.position.x / TerrainSystem.CHUNK_SIZE_X);
            tilePosY = (int)Math.Floor(transform.position.y / TerrainSystem.CHUNK_SIZE_Y);
            tilePosZ = (int)Math.Floor(transform.position.z / TerrainSystem.CHUNK_SIZE_Z);
            GameObject positionShow = GameObject.Find("UI/OtherUI/Temp/PositionShow");
            GameObject tileShow = GameObject.Find("UI/OtherUI/Temp/TileShow");
            positionShow.GetComponent<Text>().text = "x=" + transform.position.x + "y=" + transform.position.y + "z=" + transform.position.z;
            tileShow.GetComponent<Text>().text = "tx=" + tilePosX + "ty=" + tilePosY + ",tz=" + tilePosZ;

            if(m_IsEnabled)
            {
                intersectionUpdate();
                moveUpdate();
                rotationUpdate();
                skillCheckUpdate();
            }
        }

        /// <summary>
        /// 移动更新
        /// </summary>
        void moveUpdate()
        {
            float gravity = m_Gravity;
            if (controller.isGrounded)
            {
                m_IsDownAtck = false;
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);

                float speed = m_Speed;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    speed = m_Speed *5;
                }
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            if(m_IsDownAtck)
            {
                moveDirection.x = 0;
                moveDirection.z = 0;
                gravity = m_Gravity * 20;
            }
            if (isGravity)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            controller.Move(moveDirection * Time.deltaTime);

        }

        /// <summary>
        /// 旋转更新（顺便会更新第三人称相机里面的旋转参数）
        /// *右键旋转
        /// </summary>
        void rotationUpdate()
        {
            int MOUSE_RIGHT = 1;
            if (!Input.GetMouseButton(MOUSE_RIGHT))
            {
                return;
            }
            ThirdPersonCamera camera = GameObject.Find("MainCamera").GetComponent<ThirdPersonCamera>();

            Vector2 direction = camera.direction;
            float dx = Input.GetAxis("Mouse X");
            float dy = -Input.GetAxis("Mouse Y");
            direction.x += dx * 2;
            direction.y += dy * 2;
            camera.direction = direction;
            Quaternion rotate = new Quaternion();
            rotate = Quaternion.AngleAxis(direction.x, new Vector3(0, 1, 0));
            Matrix4x4 mat = Matrix4x4.Rotate(rotate);
            Vector3 dir = -mat.GetColumn(2);
            transform.rotation = rotate;

            camera.updatePose();
        }

        /// <summary>
        /// 更新交点
        /// </summary>
        void intersectionUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Varible varPosition = Varible.create(hit.point);
                    Varible varNormal = Varible.create(hit.normal);
                    String eventName = EventEnum.getEventEnumString(EventEnum.EventEnumType.TERRAIN_BLOCK_REMOVE);
                    EventSystem.get().emitEvent(eventName, varPosition, varNormal);
                }
            }
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Varible varPosition = Varible.create(hit.point);
                    Varible varNormal = Varible.create(hit.normal);
                    String eventName = EventEnum.getEventEnumString(EventEnum.EventEnumType.TERRAIN_BLOCK_ADD);
                    EventSystem.get().emitEvent(eventName, varPosition, varNormal);
                }
            }
        }


        /// <summary>
        /// 技能检测
        /// </summary>
        float doubleKeyTime = 0;
        void skillCheckUpdate()
        {
            doubleKeyTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                m_IsDownAtck = true;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                transform.position = (new Vector3(0, 100, 0));
            }

            if(Input.GetKeyDown(KeyCode.W))
            {
                if (doubleKeyTime < 0.4)
                {
                    float teleportDistance = 30;
                    Ray ray = new Ray();
                    ray.origin = transform.position;
                    ray.direction = transform.forward;
                    Vector3 destPoint = transform.position + transform.forward * teleportDistance;
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, teleportDistance))
                    {
                        destPoint = hit.point;
                    }
                    transform.position = destPoint;

                }
                doubleKeyTime = 0;
            }
        }

        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="state"></param>
        void onConsoleStateChanged(int state)
        {
            if(state == (int)ConsoleBox.ConsoleState.STATE_OPEN)
            {
                Enabled = false;
            }
            else if(state == (int)ConsoleBox.ConsoleState.STATE_CLOSE)
            {
                Enabled = true;
            }
        }
    }
}