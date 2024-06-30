import './App.css'
import { SigninForm } from './components/SigninForm/SigninForm';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

function App() {
  const queryClient = new QueryClient();

  return <>
  <QueryClientProvider client={queryClient}>
    <SigninForm />
  </QueryClientProvider>
  </>;
}

export default App
