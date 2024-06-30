import './App.css'
import { SigninForm } from './components/SigninForm/SigninForm';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { UserInfo } from './components/UserInfo/UserInfo';

function App() {
  const queryClient = new QueryClient();

  return <>
  <QueryClientProvider client={queryClient}>
    <SigninForm />
    <br/>
    <UserInfo />
  </QueryClientProvider>
  </>;
}

export default App
