import { ChangeEvent, FormEvent } from 'react';
import './SigninForm.css'

function SigninForm() {
    const [value, setValue] = useState('');

    const handleChange = (event : ChangeEvent) => {
        setValue(event.target.value)
    }

    return 
        <form onSubmit={handleSubmit}>
            <label> Name:
                <input type="text" value={value} onChange={handleChange} />
            </label>
            <label> Email:
                <input type="text" value={value} onChange={handleChange} />
            </label>
            <label> Password:
                <input type="text" value={value} onChange={handleChange} />
            </label>
            <button type="submit">Submit</button>
        </form>
}

export default SigninForm;