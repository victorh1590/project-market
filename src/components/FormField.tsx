import { FormFieldProps } from "./SigninFormTypes.ts";

const FormField = ({
  label,
  type,
  placeholder,
  name,
  register,
  error,
  valueAsNumber,
} : FormFieldProps) => (
  <>
    <label key={`${label}-label`}>{`${label}: `}</label>
    <input
      type={type}
      placeholder={placeholder}
      {...register(name, { valueAsNumber })}
    />
    {error && <><br/><span className="error-message">{error.message}</span></>}
  </>
);
export default FormField;
