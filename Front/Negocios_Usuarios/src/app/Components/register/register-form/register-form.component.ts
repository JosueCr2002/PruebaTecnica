import { Component } from '@angular/core';
import { FormGroup,Validators,FormBuilder } from '@angular/forms';
import { Datos } from 'src/app/Models/DatosRegistro';
import { DatosServiceService } from 'src/app/Services/datos-service.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent {




  FormularioRegistro:FormGroup;

  constructor( private formbuilder:FormBuilder,private servic:DatosServiceService){

    this.FormularioRegistro=this.formbuilder.group({
      Nombre:['',[Validators.required,Validators.minLength(3)]],//ejemplo nombre con 3 Ana
      Correo:['',[Validators.required,Validators.email]],
      Contrasena:['',[Validators.required,Validators.minLength(4)]],
      NombreNegocio:['',[Validators.required,Validators.minLength(5)]],
      Ruc:['',[Validators.required,Validators.minLength(14)]],
      Direccion:['',[Validators.required]]
   }) 
  }


  EnviarErrores(controlname:string, errorType:string){
    return this.FormularioRegistro.get(controlname)?.hasError(errorType)&& this.FormularioRegistro.get(controlname)?.touched
  }


  GuardarDatos(){
    
    const Tarjet: Datos = {
  
      Nombre: this.FormularioRegistro.get('Nombre')?.value,
      Correo: this.FormularioRegistro.get('Correo')?.value,
      Contrasena: this.FormularioRegistro.get('Contrasena')?.value,
      NombreNegocio: this.FormularioRegistro.get('NombreNegocio')?.value,
      Ruc: this.FormularioRegistro.get('Ruc')?.value,
      Direccion: this.FormularioRegistro.get('Direccion')?.value,
       
    }; 
    this.servic.GuardarTarjeta(Tarjet).subscribe(
      data => {
        console.log(data);
        this.FormularioRegistro.reset();
  
      },
      error => {
        console.error("Error al guardar la los Datos:", error);
      }
      );

  }

}
