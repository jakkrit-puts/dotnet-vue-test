<template>
  <div class="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-sm">
      <h2 class="mt-10 text-center text-2xl/9 font-bold tracking-tight text-gray-900">Register</h2>
    </div>

    <div class="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
      <form @submit="onSubmit" class="space-y-6">
        <div>
          <label for="username" class="block text-sm/6 font-medium text-gray-900">Username</label>
          <div class="mt-2">
            <input type="text" v-model="username" v-bind="usernameAttrs" name="username" id="username"
              autocomplete="username"
              class="block w-full  bg-white px-3 py-1.5 text-base text-gray-900 outline  -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:-outline-offset-2 sm:text-sm/6" />
            <p class="text-red-600"> {{ errors.username }}</p>
          </div>
        </div>

        <div>
          <div class="flex items-center justify-between">
            <label for="password" class="block text-sm/6 font-medium text-gray-900">Password</label>
          </div>
          <div class="mt-2">
            <input type="password" v-model="password" v-bind="passwordAttrs" name="password" id="password"
              autocomplete="current-password"
              class="block w-full  bg-white px-3 py-1.5 text-base text-gray-900 outline -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline  focus:-outline-offset-2 sm:text-sm/6" />
            <p class="text-red-600"> {{ errors.password }}</p>
          </div>
        </div>

        <div>
          <div class="flex items-center justify-between">
            <label for="password" class="block text-sm/6 font-medium text-gray-900">Confirm Password</label>
          </div>
          <div class="mt-2">
            <input type="password" v-model="confirmPassword" v-bind="confirmPasswordAttrs" name="password"
              id="confirm-password" autocomplete="current-password"
              class="block w-full  bg-white px-3 py-1.5 text-base text-gray-900 outline -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline  focus:-outline-offset-2 sm:text-sm/6" />
            <p class="text-red-600"> {{ errors.confirmPassword }}</p>
          </div>
        </div>

        <div>
          <button type="submit"
            class="flex w-full justify-center  bg-light_bg  px-3 py-1.5 text-sm/6 font-semibold  shadow-sm focus-visible:outline focus-visible:outline-offset-2 ">สมัครสมาชิก</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import axios from 'axios';
import { useToast } from 'vue-toast-notification';
import { useRouter } from 'vue-router';

const $toast = useToast();
const router = useRouter()

const { errors, handleSubmit, defineField, resetForm } = useForm({
  validationSchema: yup.object({
    username: yup.string().required(),
    password: yup.string().min(6).required(),
    confirmPassword: yup.string()
      .oneOf([yup.ref('password'), null], 'Passwords must match')
      .required('Confirm password is required')
  }),
});

const onSubmit = handleSubmit(async values => {
  try {

    const response = await axios.post("http://localhost:5076/api/User/register",
      {
        username: values.username, password: values.password
      })

    if (response.status === 201) {
      $toast.success('Register Success.');
      resetForm()

      router.push('/login')
    }

  } catch (error) {
    $toast.error(error.response.data);
  }
});

const [username, usernameAttrs] = defineField('username');
const [password, passwordAttrs] = defineField('password');
const [confirmPassword, confirmPasswordAttrs] = defineField('confirmPassword');
</script>