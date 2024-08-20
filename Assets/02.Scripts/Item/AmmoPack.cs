using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AmmoPack : MonoBehaviourPun, IItem
{
    public int ammo = 30;   //탄환

    [PunRPC]
    public void Use(GameObject target)
    {
        PlayerShooter shooter = target.GetComponent<PlayerShooter>();

        if (shooter != null && shooter.c_gun != null)
            //shooter.c_gun.remainAmmo += ammo;
            shooter.c_gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);

        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);  //모든 클라이언트에서 자신 파괴
    }
}
