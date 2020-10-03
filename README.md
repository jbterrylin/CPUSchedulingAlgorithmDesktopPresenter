# CPUSchedulingAlgorithmDesktopPresenter
Assignment of Operating System assignment. Fun. Although im not first time using c#, but im first time using .NET. Very angry when dealing drag and drop at first but getting ok in 1 day. This system done in 4 day, logic related 1 day and UI related 3 day.... No code cleaning because not enough time due to other course.

Compare with other. Mine can:
  - can choose directly see result and choose step by step(next second but not directly see result. More education purpose)
  - revert step 
  - show remaining arrvial time and bursting time.
  - have start page and title have credit (hahahaha this is my personal sick taste)

Code idea:
  - Type of algorithm
    Actually all type is almost same. Just the priority of compare which columns is different.
    So i just nid the pass the priorty of columns to child class, and main shit do on parent class. 
    For example, SJF priority is burst time first, if same then arrival time, then priority finally is cincai or take first row if 2 row is same.
    So i just need to pass (burst time, arrival time, priority)
    
  - Preemptive and Non Preemptive
    We just need to have 2 var call working process and ispreemptive.
    After process work just save process id to working process.
    Ifispreemptive is false(Mean it is nonpreemptive, working process tak boleh kacau). We just return process id in working process.
    Ifispreemptive is true, we need to think, what will effect it. There only 2 condition:
      -Process arrive
      -Process end
    So we just need to let working process become empty when process arrive and process end.
  
  - Achieve revert
    This 1 lazy to explain. Basically i just use memory to achieve this.
    
Future enhancement:
  - Clean code.
  - Add in round robin (Actually i done but i lazy so i didn't show it in UI. So other people cant use la hahahaha)
  - Add in a diary 
      - For exmaple: 
        - Second 1:
          - P1 arrive. 
          - CPU is empty. 
          - P1 is in CPU and process.
        - Second 2:
          - P2 arrive. 
          - CPU is working. 
          - P1 processing.
          - P2 waiting.
  - More credits
  - Gantchart with color.
  
but i don't think i will update it lah hahahahha
