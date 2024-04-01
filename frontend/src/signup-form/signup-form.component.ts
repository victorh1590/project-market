import { Component, inject } from '@angular/core';
import { 
  FormGroup, 
  FormControl, 
  FormsModule, 
  ReactiveFormsModule, 
  Validators, 
  AbstractControl, 
  ValidationErrors, 
  FormControlStatus,
  FormBuilder 
} from '@angular/forms';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-signup',
  templateUrl: './signup-form.component.html',
  styleUrl: './signup-form.component.scss',
  standalone : true,
  imports: [ FormsModule, ReactiveFormsModule ]
})
export class SignupFormComponent {

  private formBuilderService = inject(FormBuilder)

  signupFormGroup : FormGroup = this.formBuilderService.group({
    name : ['', 
      [ 
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(50)
      ],
      'submit'
    ],
    email : ['', 
      [
        Validators.required, 
        Validators.email
      ],
      'submit'
    ],
    sector : ['', // TODO: Check how to declare standard value for this control in Angular. 
      [
        Validators.required
      ],
      'submit'
    ],
    password : ['',
      [
        Validators.minLength(8), 
        Validators.maxLength(30),
      ],
      'submit'
    ]
  })

  // signupFormGroup : FormGroup = new FormGroup(
  //   {
  //     name : new FormControl('', {  
  //       validators: [
  //         Validators.required, 
  //         Validators.minLength(3), 
  //         Validators.maxLength(50)
  //       ],
  //       updateOn: 'submit'
  //     }),
  //     email : new FormControl('', {  
  //       validators: [
  //         Validators.required, 
  //         Validators.email
  //       ],
  //       updateOn: 'submit'
  //     }),
  //     sector : new FormControl('', {  
  //       validators: [
  //         Validators.required
  //       ],
  //       updateOn: 'submit'
  //     }),
  //     password : new FormControl('', {  
  //       validators: [
  //         Validators.minLength(8), 
  //         Validators.maxLength(30),
  //       ],
  //       updateOn: 'submit'
  //     })
  //   }
  // )

  nameIsValid : boolean = false
  emailIsValid : boolean = false
  sectorIsValid : boolean = false
  passwordIsValid : boolean = false

  // ngOnInit() 
  // {
  //   this.signupFormGroup.statusChanges.forEach((statusChanged : FormControlStatus) => {
  //     if(statusChanged === "VALID" || statusChanged === "INVALID")
  //     {

  //     }
  //   })
  // }

  getErrorMessagesForControl(controlName : FormControl) : string[]
  {

    // let nameErrors : ValidationErrors | null | undefined = this.signupFormGroup.controls.controlName[`${controlName}`]?.errors;
    return controlName.errors != null ? this.formatValidationMessage(controlName, controlName.errors) : []
  }

  formatValidationMessage(controlName : FormControl, errors : ValidationErrors) : string[]
  {
    let messages: string[] = []
    for(let error in errors)
    {
      switch(error)
      {
        case "required":
          messages.push(`The field ${controlName} is required.`);
          break;
        case "minlength":
          messages.push(`The minimum length of the field ${controlName} is ${errors['minlength'].requiredLength}.`)
          break;
        case "maxlength":
          messages.push(`The maximum length of the field ${controlName} is ${errors['maxlength'].requiredLength}.`)
          break;
        case "email":
          messages.push(`The email entered is invalid.`)
          break;
        default:
          break;
      }
    }
    return messages
  }

  submitForm()
  {
    window.alert("Form Submitted")
    this.signupFormGroup.reset()
  }


  resetForm()
  {
    this.signupFormGroup.reset();
    window.alert("Form Resetted")
  }
}