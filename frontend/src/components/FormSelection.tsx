import { FormSelectionProps } from "./SigninFormTypes.ts";

const FormSelection = ({
        label,
        multiple,
        options,
        defaultValue,
        name,
        register,
        error
    } : FormSelectionProps ) => {
        const selectOptions = options.map((option) => (
            <option key={option} value={option}>{option}</option>
        ));

        return (
        <>
            <label key={`${label}-label`}>{`${label}: `}</label>
            <select multiple={multiple} defaultValue={defaultValue} {...register(name)}>
                {selectOptions}
            </select>
            {error && <><br/><span className="error-message">{error.message}</span></>}
        </>
        )
    };
export default FormSelection;
