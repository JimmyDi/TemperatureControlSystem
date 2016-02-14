//
//////////////////////////////////
//////////////////////////////////
#include "C8051F020.h"
#include "absacc.h"
#include "data_define.c"
#include "Init_Device.c"
#include <intrins.h>
#pragma NOAREGS

#define		DELAY_1US		{_nop_();_nop_();_nop_();_nop_();}
//
//////////////////////////////////
//////////////////////////////////
#define   C2      XBYTE[0x0fff]                  //A/D
#define   C1      XBYTE[0x2fff]                  //D/A


//
//////////////////////////////////
//////////////////////////////////
#define   TIMER   0x0800                         //

//�й�CH452�궨��
//////////////////////////////////
sbit   CH452_DCLK=P2^1;                    
sbit   CH452_DIN=P2^0;		                
sbit   CH452_LOAD=P2^2;                    
sbit   CH452_DOUT=P1^5; 

#define		CH452_DCLK_SET		{CH452_DCLK=1;}
#define		CH452_DCLK_CLR		{CH452_DCLK=0;}
#define		CH452_DCLK_D_OUT	{}				
#define		CH452_DIN_SET		{CH452_DIN=1;}
#define		CH452_DIN_CLR		{CH452_DIN=0;}
#define		CH452_DIN_D_OUT		{}				
#define		CH452_LOAD_SET		{CH452_LOAD=1;}
#define		CH452_LOAD_CLR		{CH452_LOAD=0;}
#define		CH452_LOAD_D_OUT	{}				
#define		CH452_DOUT_D_IN		{CH452_DOUT=1;}

#define		DISABLE_KEY_INTERRUPT	{}
#define		ENABLE_KEY_INTERRUPT	{}
#define		CLEAR_KEY_INTER_FLAG	{}//

#define CH452_NOP		0x0000				
#define CH452_RESET     0x0201					
#define CH452_LEVEL		0x0100					
#define CH452_CLR_BIT	0x0180					
#define CH452_SET_BIT	0x01C0					// ???1,???6???
#define CH452_SLEEP		0x0202					// ??????
#define CH452_LEFTMOV   0x0300		            // ??????-??
#define CH452_LEFTCYC   0x0301		            // ??????-???
#define CH452_RIGHTMOV  0x0302		            // ??????-??
#define CH452_RIGHTCYC  0x0303		            // ??????-???	
#define CH452_SELF_BCD	0x0380					// ???BCD?,???7???
#define CH452_SYSOFF    0x0400					// ?????????
#define CH452_SYSON1    0x0401					// ????
#define CH452_SYSON2    0x0403					// ???????
#define CH452_SYSON2W   0x0423					// ???????, ??2???
#define CH452_NO_BCD    0x0500					// ????????,???3?????
#define CH452_BCD       0x0580					// ??BCD????,???3?????
#define CH452_TWINKLE   0x0600		            // ??????,???8???
#define CH452_GET_KEY	0x0700					// ????,??????
#define CH452_DIG0      0x0800					// ????0??,???8???
#define CH452_DIG1      0x0900		            // ????1??,???8???
#define CH452_DIG2      0x0a00		            // ????2??,???8???
#define CH452_DIG3      0x0b00		            // ????3??,???8???
#define CH452_DIG4      0x0c00		            // ????4??,???8???
#define CH452_DIG5      0x0d00					// ????5??,???8???
#define CH452_DIG6      0x0e00					// ????6??,???8???
#define CH452_DIG7      0x0f00		            // ????7??,???8???

#define		CH452_BCD_SPACE		0x10
#define		CH452_BCD_PLUS		0x11
#define		CH452_BCD_MINUS		0x12
#define		CH452_BCD_EQU		0x13
#define		CH452_BCD_LEFT		0x14
#define		CH452_BCD_RIGHT		0x15
#define		CH452_BCD_UNDER		0x16
#define		CH452_BCD_CH_H		0x17
#define		CH452_BCD_CH_L		0x18
#define		CH452_BCD_CH_P		0x19
#define		CH452_BCD_DOT		0x1A
#define		CH452_BCD_SELF		0x1E
#define		CH452_BCD_TEST		0x88
#define		CH452_BCD_DOT_X		0x80

#define		CH452_KEY_MIN		0x40
#define		CH452_KEY_MAX		0x7F




//extern	unsigned char CH452_Read(void);		// ?CH452??????
//extern  void CH452_Write(unsigned short cmd);	

//ȫ�ֱ�������
//////////////////////////////////
//////////////////////////////////
bit controlbutton;
bit controlyanchi_tingzhi_button;
bit controlyanchi_qidong_button;

unsigned char   status_hand;


uchar yanchi_ting[2];
uchar yanchi_qi[2];
uchar yanchi_num=0;
uchar yanchi_num_go=0;

uint chuanru[3];                         //�����洢�����������
int flag=0;								 //�����õı�־λ

uint temp_now_ori;						 //������ȡA/Dת���¶�ֵ
int temp_answer;                        //���ڷ����¶�ֵ
uint controlflag;                      //�¶ȿ��Ʒ�ʽ



//��ʼ������
////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////


//CH452�ĳ�ʼ��
//////////////////////////////////
void CH452Init(void)
{

CH452_DCLK=0;
CH452_DCLK=1;
CH452_DIN=1;
CH452_LOAD=1;

}

//��ʱ���ĳ�ʼ������ʱ��1������ɴ��ڵĲ��������ã���ʱ��0��������ӳٹ���
//////////////////////////////////
void Init_Time()		   //
{
   TMOD=0x20;		      //
   TH1=0xFD;		      //
   TL1=0xFD;
   TR1=1; 			    //
}

//COM�ڵĳ�ʼ��
//////////////////////////////////
void Init_Com()	      
{
   SCON0=0x50;	    //�����ڷ�ʽ1�������������
   PCON=0x00;		//�����ʱ���Ϊ1
}

//�жϵĳ�ʼ��
//////////////////////////////////
void Init_Interrupt()	
{
         EA=1;       //�����ж�    
		 ES0=1;	  	 //�򿪴����ж�
}






//���ܺ���
////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////


//CH452д
//////////////////////////////////
void CH452_Write(unsigned short cmd)
{
	unsigned char i;
	DISABLE_KEY_INTERRUPT;		  //
	CH452_LOAD_CLR;                 //
	CH452_LOAD_D_OUT;		// 
	CH452_DOUT_D_IN;		//
	CH452_DIN_D_OUT;		// 
	CH452_DCLK_D_OUT;		// 
	for(i=0;i!=12;i++)   	//
	{
		if (cmd&1) {CH452_DIN_SET;}
		else {CH452_DIN_CLR;}  // 
//		CH452_DIN=cmd&1;
    	CH452_DCLK_CLR;
    	cmd>>=1;
    	CH452_DCLK_SET;        //
	}
  	CH452_LOAD_SET;         //
	DELAY_1US;				//
	DELAY_1US;
	DELAY_1US;
	DELAY_1US;
	DELAY_1US;
	DELAY_1US;
  	ENABLE_KEY_INTERRUPT;
}




//CH452��
//////////////////////////////////
unsigned char CH452_Read( void )
{
	unsigned char i;
  	unsigned char cmd,keycode;		//
  	DISABLE_KEY_INTERRUPT;		    //	
  	cmd=0x07;			           //
  	CH452_LOAD_CLR;
  	for(i=0;i!=4;i++)  // 
	{
		if (cmd&1) {CH452_DIN_SET;}
		else {CH452_DIN_CLR;}  // 
//		CH452_DIN=cmd&1;		      //
    	CH452_DCLK_CLR;			 
    	cmd>>=1;			      //
    	CH452_DCLK_SET;			      //
 	}
  	CH452_LOAD_SET;				      //
  	keycode=0;				      //
  	for(i=0;i!=7;i++)
	{
		keycode<<=1;			      //
		if (CH452_DOUT) keycode++;      //
//    	keycode|=CH452_DOUT;
    	CH452_DCLK_CLR;			      //
    	CH452_DCLK_SET;
 	}
  	CLEAR_KEY_INTER_FLAG;	     //
  	ENABLE_KEY_INTERRUPT;
  	return(keycode);			     //
}


//�ӳٺ���
//////////////////////////////////
void delayms(unsigned int i)
{	
    unsigned int	j;
	do
	   {	for(j=0;j!=1000;j++)
		       {;}
	   }
	while(--i);

}

//�¶ȵ���ʾ���ϴ�����
//////////////////////////////////
int showtemp_answer(void)		                        //�ú����������¶���ʾ���ѯ����
{
	unsigned short x1=0,x2=0;
	int temp=0,jinwei=0;

	C2=temp_now_ori;								
	delay1();
    temp_now_ori=C2;
	
	
	temp=temp_now_ori*100/256;
	jinwei=temp_now_ori*100%256;
	if(jinwei>=128)
	{temp=temp+1;}

	x1=y%10;
	x2=y/10;

	CH452_Write(CH452_DIG3 | x1);
	CH452_Write(CH452_DIG2 | x2);

	delayms(50);
	return temp;
}

//���ݷ��ͺ���
//////////////////////////////////
void send_data(uchar message)	                
{
   SBUF0=message;
   while(TI0==0);
   TI0=0;
}

//�ӳٺ���
//////////////////////////////////
void delay1(void)                                //
{ 
    unsigned int i;
    for(i=0;i<TIMER;++i);
}





//�¶ȿ��ƺ���                                controlflag=1   �趨���¶ȿ���
//                                            controlflag=2    �¶���1����
//                                            controlflag=3    �¶ȼ�1����
//                                            controlbutton    �¶ȿ���ʹ�ܿ���
//////////////////////////////////
void temp_control(int temp_flag)                           
  {	  
	
	    int temp,xianshi_temp;
        if(controlflag==1)
	       {	 
                 C2=xianshi_temp;								
			     delay1();
			     xianshi_temp=C2;   
	      
				 temp=showtemp_answer(xianshi_temp);
		
		         if(chuanru[1]>temp)
		            {
		              C1=0xff;
		            }

	       	      else if(chuanru[1]<temp)
		            {
		              C1=0x00;
		            }
	             /* else if((chuanru[1]>=temp-3)&&(chuanru[1]<=temp+3))
		            {
		              C1=0x80;
				    }	       		
				 */
		   }
   
   		if(controlflag==2)
		   {
		   	     C2=xianshi_temp;								
			     delay1();
			     xianshi_temp=C2;   
	      
				 temp=showtemp_answer(xianshi_temp);
		
		         if((temp_answer+1)>temp)
		            {
		              C1=0xff;
		            }

	       	     else if((temp_answer+1)<temp)
		            {
		              C1=0x00;
		            }
		   }
   
   		if(controlflag==3)
   		   {
		   		 C2=xianshi_temp;							
			     delay1();
			     xianshi_temp=C2;   
	      
				 temp=showtemp_answer(xianshi_temp);
		
		         if((temp_answer-1)>temp)
		            {
		              C1=0xff;
		            }

	       	     else if((temp_answer-1)<temp)
		            {
		              C1=0x00;
		            }
		   }
   
   }

//�ӳٺ���
//////////////////////////////////
void time_delay()
   {
   	                               	
		ET0=1;		       
        TR0=1;
		TH0=(8192-4607)/32;
        TL0=(8192-4607)%32;
											 
		     
   }



//�жϺ���
////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////


//��ʱ���ж�1
//////////////////////////////////
void T0_delay()interrupt 1     //?????��??????0????1s
{
     if(controlyanchi_qidong_button==1)
	   {
	     if(yanchi_num_go!=0)
	       {
   	         TH0=(8192-4607)/32;
             TL0=(8192-4607)%32;
	         yanchi_num_go--;
			 if(yanchi_qi[1]==0)
			    {
				  yanchi_qi[1]=60;
				  yanchi_qi[0]--;
				}
			 else yanchi_qi[1]--;
             
		   }   
	     else if(yanchi_num_go==0)
			{
			  ET0=0;		       
              TR0=0; 
			  controlbutton=1;
			  controlyanchi_qidong_button=0;
	        }
		
		
	  }
	 if(controlyanchi_qidong_button==1)
	   {
	     if(yanchi_num_go!=0)
	       {
   	         TH0=(8192-4607)/32;
             TL0=(8192-4607)%32;
	         yanchi_num_go--;
             if(yanchi_ting[1]==0)
			    {
				  yanchi_ting[1]=60;
				  yanchi_ting[0]--;
				}
			 else yanchi_ting[1]--;
		   
		   }   
	     else if(yanchi_num_go==0)
			{
			  ET0=0;		       
              TR0=0; 
			  controlbutton=0;
			  controlyanchi_qidong_button=0;
	        }
		
		
	  }
}


//��ʱ���ж�4
//////////////////////////////////
void com() interrupt 4
{
   TR0=0;
   RI0=0;
   temp=SBUF0;

    switch(flag) //����Ϊ���˲��֣�flag=5Ϊѡ����������
    {
        case 0:
	              if(temp=='C')
				     flag=1;
	              break;
        case 1:
                  if(temp=='O')
				      flag=2;
                  else flag=0;
	              break;
	    case 2:
		          if(temp=='M')
				      flag=3;
	              else flag=0;
	              break;
	    case 3:
		          chuanru[0]=temp;
		          flag=4; 
		          break;
	    case 4:
		          chuanru[1]=temp;
		          flag=5;
		          break;
	    case 5:
		          chuanru[2]=temp;
		          switch(chuanru[0])
                    {
                         case 0xA0:					   //�����¶ȿ�������
                                    controlbutton=1;
									controlflag=1;
									break;							   
						 case 0xA1: 				   //�¶�ֵ��1����
                                            
								    controlbutton=1;
								    controlflag=2
									break;
						 case 0xA2:					   //�¶�ֵ��1����
								    controlbutton=1;
								    controlflag=3;
									break;
					     case 0xA3:		               //����/ֹͣ��������
								    controlbutton=!controlbutton;
								    break;
					     case 0xA4:					   //�ӳ�����ʱ������
								    controlyanchi_qidong_button=1;
									controlbutton=0;	   
								    
									yanchi_qi[0]=chuanru[1];
									yanchi_qi[1]=chuanru[2];
									
									yanchi_num=2*(chuanru[1]*60+chuanru[2]);
								    yanchi_num_go=yanchi_num;
										   
								    break;
						 case 0xA5:					   //�ӳ�ֹͣʱ���ѯ
								    controlyanchi_tingzhi_button=1;
									controlbutton=1;
										  
								    yanchi_ting[0]=chuanru[1];
									yanchi_ting[1]=chuanru[2];
								    
									yanchi_num=2*(chuanru[1]*60+chuanru[2]);
									yanchi_num_go=yanchi_num;

                                    break;
						 case 0xA8:					   //��ǰ�¶Ȳ�ѯ����
								          	temp_answer=showtemp_answer();
										    send_data('C');
                                            send_data('O');
                                            send_data('M');
                                            send_data(0xA8);
                                            send_data(temp_answer);
                                            send_data(0x00);
										  break;
						 case 0xAA:			          //�ӳ�����ʱ���ѯ
								          	send_data('C');
                                            send_data('O');
                                            send_data('M');
                                            send_data(0xAA);
                                            send_data(yanchi_qi[0]);
                                            send_data(yanchi_qi[1]);
										  break;
					      case 0xAB:		         //�ӳ�ֹͣʱ���ѯ
								          	send_data('C');
                                            send_data('O');
                                            send_data('M');
                                            send_data(0xAB);
                                            send_data(yanchi_ting[0]);
                                            send_data(yanchi_ting[1]);
										  break;
						case 0xAE:					   //���ƹ���״̬��ѯ
								           if(controlbutton)
										  	 {
											   status_hand=status_hand|0x80;
                                             }
										   if(controlyanchi_qidong_button)
										  	 {
											   status_hand=status_hand|0x40;
											 }
										   if(controlyanchi_tingzhi_button)
										     {
											   status_hand=status_hand|0x20;
											 }
                                           	send_data('C');
                                            send_data('O');
                                            send_data('M');
                                            send_data(0xAE);
                                            send_data(status_hand);
                                            send_data(0xff);
										  break;
							   }
		flag=0;
		break;
        
                  

      
                
    }

    TR0=1;
}







 //������
 //////////////////////////////////
 //////////////////////////////////
void main(void)							
 {
	Init_Device();
   	Init_Time();
	Init_Com();												 
    Init_Interrupt(); 
   
	CH452Init();
	CH452_Write(CH452_RESET);
	CH452_Write(CH452_SYSON2);	
	CH452_Write(CH452_BCD);  
	CH452_Write(CH452_DIG3 | 0);
	
    delay1();
	
    chuanru[0]=0;
	chuanru[1]=0;
	chuanru[2]=0;

    yanchi_ting[0]=0;
	yanchi_ting[1]=0;

    yanchi_qi[0]=0;
    yanchi_qi[1]=0;

    controlflag=0;
	controlbutton=0;
	controlyanchi_tingzhi_button=0;
    controlyanchi_qidong_button=0;
	status_hand=0x00;
	C1=0x80;
	   
	while(1)
		 {
	                 
              showtemp_answer(); 	
       		  
              if(controlyanchi_qidong_button)  //�ӳ���������
			   	  {
				  	time_delay();
				  }
			   if(controlyanchi_tingzhi_button)	//�ӳ�ֹͣ����					  	
				  {
					time_delay();
				  }
			  if(controlbutton)    			    //�¶ȿ���
               	 {
				     if(controlflag==1)
				 	   {
					     temp_control(controlflag);												 
					   }
				 	 else if(controlflag==2)
					   {
					     temp_control(controlflag);
					   }
					 else if(controlflag==3)
					   {
					     temp_control(controlflag);
					   }
				 }
			}   
 }
