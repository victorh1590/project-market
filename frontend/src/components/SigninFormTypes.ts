import { OptionHTMLAttributes } from "react";
import { FieldError, UseFormRegister } from "react-hook-form";

export type FormData = {
    nome: string;
    email: string;
    senha: string;
    setor: string;
  };

  export type FormFieldProps = {
    label: string;
    type: string;
    placeholder: string;
    name: ValidFieldNames;
    register: UseFormRegister<FormData>;
    error: FieldError | undefined;
    valueAsNumber?: boolean;
  };
  
  export type FormSelectionProps = {
    label: string;
    multiple: boolean;
    options: Array<string>;
    defaultValue: string;
    name : ValidSelectionNames;
    register: UseFormRegister<FormData>;
    error: FieldError | undefined;
    // valueAsNumber?: boolean;
  }

  export type ValidSelectionNames = 
  | "setor"

  export type ValidFieldNames =
  | "nome"
  | "email"
  | "senha"