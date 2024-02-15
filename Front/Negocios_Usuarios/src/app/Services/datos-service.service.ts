import { Injectable } from '@angular/core';
import { Datos } from '../Models/DatosRegistro';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DatosServiceService {
 
  

  constructor( private _httpclient:HttpClient) { }

  UrlBase="https://localhost:7123/api/Api/";


  GuardarTarjeta(Data: Datos):  Observable<Datos> {
    return this._httpclient.post<Datos>(`${this.UrlBase}`, Data)
     
  }
}
