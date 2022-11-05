using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkSucker : MonoBehaviour
{
   
    
    

    private bool suckingIn = false;




    private void Update()
    {
        if (suckingIn)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
       if(suckingIn == true)
        {
            Debug.Log("Collider is disabled");
            Destroy(gameObject, 2f);
        }
    }



    IEnumerator OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("HandSinker"))
        {

            Debug.Log("hitSinkerHand");
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (other.CompareTag("Ground"))
        {
            Debug.Log("Fell on Ground");
            yield return new WaitForSeconds(3);
            suckingIn = true;
            
            //  transform.Translate(speed * (-1) * Time.deltaTime, 0 , 0 , Space.Self);

        }

        

        
    }

    

}
