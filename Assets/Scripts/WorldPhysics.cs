using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using System.Numerics;
using UnityEngine.UI;
using UnityEngine;


public class WorldPhysics : MonoBehaviour
{
    // Homework: Incorporate launch position, launch speed, and launch angle into our new custom-physics system!
    // Plot 3D projectile motion by specifying a pitch (launch-angle about X) and yaw (launch-angle about Y)
    public Vector3 launchPosition;
    public float dt;
    public Vector3 grav = new Vector3(0, -9.8f, 0);
    public List<Body> bodies = new List<Body>();

    public int points = 0;
    public Text pointsText;

    void checkForNewBodies()
    {
        Body[] allObjects = FindObjectsOfType<Body>(false);
        foreach (Body BodyFound in allObjects)
        {
            if (!bodies.Contains(BodyFound))
            {
                bodies.Add(BodyFound);
            }
        }
    }
    bool checkSphereSphereCollision(Body bodyA, Body bodyB)
    {
        Vector3 displacement = bodyA.transform.position - bodyB.transform.position;
        float distance = displacement.magnitude;
        return distance < bodyA.radius;
    }
    bool checkSpherePlaneCollision(Body bodyA, Body bodyB)
    {
        Vector3 normal = bodyB.transform.rotation * new Vector3(0, 1, 0);
        Vector3 displacement = bodyA.transform.position - bodyB.transform.position;
        float projection = Vector3.Dot(displacement, normal);
        return Mathf.Abs(projection) < bodyA.radius;
    }
    bool checkSphereHalfPlaneCollision(Body bodyA, Body bodyB)
    { 
        Vector3 normal = bodyB.transform.rotation * new Vector3(0,1,0);
        Vector3 displacement = bodyA.transform.position - bodyB.transform.position;
        float projection = Vector3.Dot(displacement, normal);
        return projection < bodyA.radius;
    }

    Body Fix(Body bodyA, Body bodyB) 
    {
        bodyA.isProjectile = false;
        Vector3 normal = bodyB.transform.rotation * new Vector3(0, 1, 0);
        Vector3 displacement = bodyA.transform.position - bodyB.transform.position;
        float projection = Vector3.Dot(displacement, normal);
        bodyA.transform.position += normal * (bodyA.radius - projection);

        return bodyA;
    }
    private void checkCollision()
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            Body bodyA = bodies[i];
            for (int j = 0; j < bodies.Count; j++)
            {
                Body bodyB = bodies[j];
                //checks for collision detection type
                if (bodyA.GetShape() == 0 && bodyB.GetShape() == 0)
                {
                    if (checkSphereSphereCollision(bodyA, bodyB))
                    {
                        if (bodyA.ObjectType == 1 && bodyB.ObjectType == 2)
                        {
                            Debug.Log("Hit");
                            Destroy(bodyB.gameObject);
                            bodies.Remove(bodyB);
                            points += 1;
                            //delete bodyB and add +1 to the score
                        }
                        else if(bodyA.ObjectType == 2 && bodyB.ObjectType == 1)
                        {
                            Debug.Log("Hit");
                            Destroy(bodyA.gameObject);
                            bodies.Remove(bodyA);
                            points += 1;
                            //delete bodyA and add +1 to the score
                        }
                    }
                    else
                    {
                    }
                }
                else if (bodyA.GetShape() == 0 && bodyB.GetShape() == 2) 
                {
                    if (checkSpherePlaneCollision(bodyA, bodyB))
                    {
                        if (bodyA.isProjectile) 
                        {
                            bodyA = Fix(bodyA, bodyB);
                        }
                    }
                    else
                    {
                    }
                }
                else if (bodyB.GetShape() == 0 && bodyA.GetShape() == 2)
                {
                    if (checkSpherePlaneCollision(bodyB, bodyA))
                    {
                    }
                    else
                    {
                    }
                }
                else if (bodyA.GetShape() == 0 && bodyB.GetShape() == 3)
                {
                    if (checkSphereHalfPlaneCollision(bodyA, bodyB))
                    {
                        if (bodyA.isProjectile)
                        {
                            bodyA = Fix(bodyA, bodyB);
                        }
                    }
                    else
                    {
                    }
                }
                else if (bodyB.GetShape() == 0 && bodyA.GetShape() == 3)
                {
                    if (checkSphereHalfPlaneCollision(bodyB, bodyA))
                    {
                    }
                    else
                    {
                    }
                }
            }
        }
    }
    private void Start()
    {
        dt = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        checkForNewBodies();
        foreach (Body body in bodies)
        {
            if (body.isProjectile)
            {
                body.Simulate(grav, dt);
                body.transform.localPosition += new Vector3(
                    (body.vel.x * dt) * body.drag,
                    (body.vel.y * dt) * body.drag,
                    (body.vel.z * dt) * body.drag);
            }
        }
        checkCollision();
        UpdatePointsDisplay();
    }

    void UpdatePointsDisplay()
    {
        if(pointsText != null)
        {
            pointsText.text = "Points: " + points.ToString();
        }
    }
}