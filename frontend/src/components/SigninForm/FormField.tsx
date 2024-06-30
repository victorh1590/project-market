import { FormFieldProps } from "./SigninFormTypes.ts";

export const FormField = ({
  label,
  type,
  defaultValue,
  name,
  register,
  error,
  valueAsNumber,
} : FormFieldProps) => (
  <>
    <label key={`${label}-label`}>{`${label}: `}</label>
    <input
      type={type}
      placeholder={defaultValue}
      {...register(name, { valueAsNumber })}
    />
    {error && <><br/><span className="error-message">{error.message}</span></>}
  </>
);