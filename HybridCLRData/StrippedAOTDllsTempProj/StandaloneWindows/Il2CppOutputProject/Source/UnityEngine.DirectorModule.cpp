#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>



struct Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87;
struct Action_1_tB645F646DB079054A9500B09427CB02A88372D3F;
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
struct AsyncCallback_t7FEF460CBDCFB9C5FA2EF776984778B9A4145F4C;
struct DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E;
struct IAsyncResult_t7B9B5A0ECB35DCEC31B8A8122C37D687369253B5;
struct MethodInfo_t;
struct PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475;
struct String_t;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;

struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t30BE97772842985A3AF723FA1759387A3E79C563 
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct Delegate_t  : public RuntimeObject
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	RuntimeObject* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	bool ___method_is_virtual;
};
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr;
	intptr_t ___invoke_impl;
	Il2CppIUnknown* ___m_target;
	intptr_t ___method;
	intptr_t ___delegate_trampoline;
	intptr_t ___extra_arg;
	intptr_t ___method_code;
	intptr_t ___interp_method;
	intptr_t ___interp_invoke_impl;
	MethodInfo_t* ___method_info;
	MethodInfo_t* ___original_method_info;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data;
	int32_t ___method_is_virtual;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr;
};
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};
struct MulticastDelegate_t  : public Delegate_t
{
	DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771* ___delegates;
};
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates;
};
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates;
};
struct Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87  : public MulticastDelegate_t
{
};
struct Action_1_tB645F646DB079054A9500B09427CB02A88372D3F  : public MulticastDelegate_t
{
};
struct Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};
struct PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475  : public Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA
{
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___played;
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___paused;
	Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* ___stopped;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif


IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Action_1_Invoke_mF2422B2DD29F74CE66F791C3F68E288EC7C3DB9E_gshared_inline (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) ;

inline void Action_1_Invoke_m1DDC149E3BDDF7CAA1CCEBDA6D58D6971F126303_inline (Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* __this, PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475* ___0_obj, const RuntimeMethod* method)
{
	((  void (*) (Action_1_tB645F646DB079054A9500B09427CB02A88372D3F*, PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475*, const RuntimeMethod*))Action_1_Invoke_mF2422B2DD29F74CE66F791C3F68E288EC7C3DB9E_gshared_inline)(__this, ___0_obj, method);
}
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void PlayableDirector_SendOnPlayableDirectorPlay_m7F75DBA4355DAA92F53AC337BB952069B63081A0 (PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475* __this, const RuntimeMethod* method) 
{
	bool V_0 = false;
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_0 = __this->___played;
		V_0 = (bool)((!(((RuntimeObject*)(Action_1_tB645F646DB079054A9500B09427CB02A88372D3F*)L_0) <= ((RuntimeObject*)(RuntimeObject*)NULL)))? 1 : 0);
		bool L_1 = V_0;
		if (!L_1)
		{
			goto IL_001b;
		}
	}
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_2 = __this->___played;
		NullCheck(L_2);
		Action_1_Invoke_m1DDC149E3BDDF7CAA1CCEBDA6D58D6971F126303_inline(L_2, __this, NULL);
	}

IL_001b:
	{
		return;
	}
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void PlayableDirector_SendOnPlayableDirectorPause_m1B8EE7CBD23957C664AA417A9261194DFFFADFE1 (PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475* __this, const RuntimeMethod* method) 
{
	bool V_0 = false;
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_0 = __this->___paused;
		V_0 = (bool)((!(((RuntimeObject*)(Action_1_tB645F646DB079054A9500B09427CB02A88372D3F*)L_0) <= ((RuntimeObject*)(RuntimeObject*)NULL)))? 1 : 0);
		bool L_1 = V_0;
		if (!L_1)
		{
			goto IL_001b;
		}
	}
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_2 = __this->___paused;
		NullCheck(L_2);
		Action_1_Invoke_m1DDC149E3BDDF7CAA1CCEBDA6D58D6971F126303_inline(L_2, __this, NULL);
	}

IL_001b:
	{
		return;
	}
}
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void PlayableDirector_SendOnPlayableDirectorStop_m4E9AEB579B8EA66ECC6FA9BE23BBF7973AB3EDD7 (PlayableDirector_t895D7BC3CFBFFD823278F438EAC4AA91DBFEC475* __this, const RuntimeMethod* method) 
{
	bool V_0 = false;
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_0 = __this->___stopped;
		V_0 = (bool)((!(((RuntimeObject*)(Action_1_tB645F646DB079054A9500B09427CB02A88372D3F*)L_0) <= ((RuntimeObject*)(RuntimeObject*)NULL)))? 1 : 0);
		bool L_1 = V_0;
		if (!L_1)
		{
			goto IL_001b;
		}
	}
	{
		Action_1_tB645F646DB079054A9500B09427CB02A88372D3F* L_2 = __this->___stopped;
		NullCheck(L_2);
		Action_1_Invoke_m1DDC149E3BDDF7CAA1CCEBDA6D58D6971F126303_inline(L_2, __this, NULL);
	}

IL_001b:
	{
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Action_1_Invoke_mF2422B2DD29F74CE66F791C3F68E288EC7C3DB9E_gshared_inline (Action_1_t6F9EB113EB3F16226AEF811A2744F4111C116C87* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, RuntimeObject*, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl)((Il2CppObject*)__this->___method_code, ___0_obj, reinterpret_cast<RuntimeMethod*>(__this->___method));
}
