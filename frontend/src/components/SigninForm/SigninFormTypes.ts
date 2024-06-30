import { FieldError, UseFormRegister } from "react-hook-form";

export type SigninFormData = {
  nome: string;
  email: string;
  senha: string;
  setor: string;
};

export type FormFieldProps = {
  label: string;
  type: string;
  defaultValue: string;
  name: ValidFieldNames;
  register: UseFormRegister<SigninFormData>;
  error: FieldError | undefined;
  valueAsNumber?: boolean;
};
  
export type FormSelectionProps = {
  label: string;
  multiple: boolean;
  options: Array<string>;
  defaultValue: string;
  name : ValidSelectionNames;
  register: UseFormRegister<SigninFormData>;
  error: FieldError | undefined;
  // valueAsNumber?: boolean;
}

export type ValidSelectionNames = 
| "setor"

export type ValidFieldNames =
| "nome"
| "email"
| "senha"