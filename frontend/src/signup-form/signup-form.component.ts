import { Component } from '@angular/core';
import { FormGroup, FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-signup',
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.scss',
  standalone : true,
  imports: [ FormsModule, ReactiveFormsModule ]
})
export class SignupFormComponent {
  signupFormGroup : FormGroup = new FormGroup(
    {
      name : new FormControl(''),
      email : new FormControl(''),
      sector : new FormControl(''),
      password : new FormControl('')
    }
  )

  submitForm()
  {
    window.alert("Form Submitted")
  }


  resetForm()
  {
    this.signupFormGroup.reset
    window.alert("Form Resetted")
  }
}